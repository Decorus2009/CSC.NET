namespace hw1
{
    public interface ITrie
    {
        // Expected complexity: O(|element|)
        // Returns true if this set did not already contain the specified element
        bool Add(string element);

        // Expected complexity: O(|element|)
        bool Contains(string element);

        // Expected complexity: O(|element|)
        // Returns true if this set contained the specified element
        bool Remove(string element);

        // Expected complexity: O(1)
        int Size();

        // Expected complexity: O(|prefix|)
        int HowManyStartsWithPrefix(string prefix);
    }
}