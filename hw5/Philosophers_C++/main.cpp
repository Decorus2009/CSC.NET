#include <atomic>
#include <condition_variable>
#include <cstdio>
#include <cstdlib>
#include <deque>
#include <iostream>
#include <mutex>
#include <random>
#include <thread>
#include <vector>

unsigned debug_flag;

void sleep_(unsigned milliseconds) {
    std::this_thread::sleep_for(std::chrono::milliseconds(milliseconds));
}

class Fork {
public:
    Fork() {}

    bool try_take() {
        return mutex.try_lock();
    }

    void put() {
        mutex.unlock();
    }

private:
    std::mutex mutex;
};

typedef std::chrono::microseconds Microseconds;
typedef std::chrono::steady_clock Clock;
typedef Clock::time_point Time;

class Philosopher {
public:
    Philosopher(unsigned id, Fork *fork_left, Fork *fork_right,
                unsigned think_delay, unsigned eat_delay)
            : id(id), fork_left(fork_left), fork_right(fork_right),
              r_engine(std::random_device()()), think_delay_dist(0, think_delay),
              eat_delay_dist(0, eat_delay), eat_count(0), wait_time(0),
              stop_flag(false) {}

    void run(bool left_right) {
        unsigned max_wait_time_ms = 10;

        while (!stop_flag) {

            think();

            while (true) {
                if (!fork_left->try_take()) {
                    sleep_(rand() % max_wait_time_ms);
                    continue;
                }
                if (debug_flag) std::printf("[%u] took left fork\n", id);

                if (!fork_right->try_take()) {
                    if (debug_flag) std::printf("[%u] put left fork\n", id);
                    fork_left->put();
                    sleep_(rand() % max_wait_time_ms);
                    continue;
                }
                if (debug_flag) std::printf("[%u] took right fork\n", id);
                break;
            }

            eat();

            if (debug_flag) std::printf("[%u] put right fork\n", id);
            fork_right->put();
            if (debug_flag) std::printf("[%u] put left fork\n", id);
            fork_left->put();
            sleep_(rand() % max_wait_time_ms);
        }
        if (debug_flag) std::printf("[%u] stopped\n", id);
    }

    void stop() { stop_flag = true; }

    void printStats() const {
        std::printf("[%u] %u %lld\n", id, eat_count, wait_time);
    }

private:
    void think() {
        if (debug_flag) std::printf("[%u] thinking\n", id);
        sleep_(think_delay_dist(r_engine));
        if (debug_flag) std::printf("[%u] hungry\n", id);
        wait_start = Clock::now();
    }

    void eat() {
        wait_time +=
                std::chrono::duration_cast<Microseconds>(Clock::now() - wait_start)
                        .count();
        if (debug_flag) std::printf("[%u] eating\n", id);
        sleep_(eat_delay_dist(r_engine));
        ++eat_count;
    }

    Fork *fork_left;
    Fork *fork_right;
    std::default_random_engine r_engine;
    std::uniform_int_distribution<unsigned> think_delay_dist;
    std::uniform_int_distribution<unsigned> eat_delay_dist;
    Time wait_start;
    unsigned id;
    long long wait_time;
    unsigned eat_count;

    std::atomic<bool> stop_flag;
};

int main(int argc, char *argv[]) {
    if (argc != 6) {
        std::fprintf(
                stderr,
                "Usage: %s phil_count duration think_delay eat_delay debug_flag\n",
                argv[0]);
        return 1;
    }

    unsigned N = std::atoi(argv[1]);
    unsigned duration = std::atoi(argv[2]);
    unsigned think_delay = std::atoi(argv[3]);
    unsigned eat_delay = std::atoi(argv[4]);
    debug_flag = std::atoi(argv[5]);

    std::setvbuf(stdout, NULL, _IONBF, BUFSIZ);

    // we use deques to avoid defining copy/move constructors
    std::deque<Fork> forks(N);
    std::deque<Philosopher> phils;

    for (unsigned i = 0; i < N; ++i) {
        phils.emplace_back(i + 1, &forks[(i + 1) % N], &forks[i], think_delay, eat_delay);
    }

    std::vector<std::thread> threads;
    threads.reserve(N);

    for (auto it = phils.begin(); it != phils.end(); it++) {
        // first philosopher takes right fork first
        bool left_right = !(it == phils.begin());
        threads.emplace_back([it, left_right] { it->run(left_right); });
    }

    sleep_(duration * 1000);

    for (auto &phil : phils) {
        phil.stop();
    }
    for (auto &thread : threads) {
        thread.join();
    }
    for (const auto &phil : phils) {
        phil.printStats();
    }
}