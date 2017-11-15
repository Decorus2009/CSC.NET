using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimeNumbers
{
    class PrimeNumberTaskControls
    {
        private TextBox numberTextBox;
        private Button runButton;
        private Button cancelButton;
        private Label statusLabel;
        private ProgressBar progressBar;
        private Label resultLabel;
        private CancellationTokenSource cancellationTokenSource;

        public PrimeNumberTaskControls(TableLayoutPanel tableLayoutPanel, int row)
        {
            numberTextBox = CreateNumberTextBox();
            runButton = CreateRunButton();
            cancelButton = CreateCancelButton();
            statusLabel = CreateStatusLabel();
            progressBar = CreateProgressBar();
            resultLabel = CreateResultLabel();

            runButton.Click += new EventHandler(runButton_Click);
            cancelButton.Click += new EventHandler(cancelButton_Clicked);

            tableLayoutPanel.Controls.Add(numberTextBox, 0, row);
            tableLayoutPanel.Controls.Add(runButton, 1, row);
            tableLayoutPanel.Controls.Add(cancelButton, 2, row);
            tableLayoutPanel.Controls.Add(statusLabel, 3, row);
            tableLayoutPanel.Controls.Add(resultLabel, 5, row);
            tableLayoutPanel.Controls.Add(progressBar, 4, row);
        }


        private async void runButton_Click(object sender, EventArgs e)
        {
            long number;
            try
            {
                number = long.Parse(numberTextBox.Text);
            }
            catch (Exception ignored)
            {
                MessageBox.Show("Cannot parse number in number text box (maybe the number is too big)");
                return;
            }
            if (number < 0)
            {
                MessageBox.Show("Number must be positive");
                return;
            }

            // This lambda is executed in context of UI thread, so it can safely update form controls
            var progress = new Progress<int>(v => progressBar.Value = v);

            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            var task = Task.Run(() => CountPrimeNumbersFor(number, token, progress), token);
            try
            {
                statusLabel.Text = "WAITING";

                await task;

                statusLabel.Text = "COMPLETED";
                resultLabel.Text = task.Result.ToString();
            }
            // при отмене из async метода вылетает OperationCanceledException, а не AggregateException
            catch (OperationCanceledException ignored)
            {
                statusLabel.Text = "CANCELED";
                progressBar.Value = 0;
                resultLabel.Text = "...";
            }
        }

        private void cancelButton_Clicked(object sender, EventArgs e)
        {
            // должно работать только внутри runButton_Click
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
        }

        private int CountPrimeNumbersFor(long number, CancellationToken token, IProgress<int> progress)
        {
            // из документации: Executes the specified delegate on the thread that owns the control's underlying window handle
            // из данного потока, в котором выполняется задача, нельзя модифицировать элементы GUI, поэтому такое решение
            // https://stackoverflow.com/questions/661561/how-do-i-update-the-gui-from-another-thread-in-c/18033198#18033198
            // блокирует рабочий поток, но только 1 раз
            // Running on the UI thread
            statusLabel.Invoke((MethodInvoker)delegate
            {
                statusLabel.Text = "RUNNING";
            });

            var counter = 0;
            for (long i = 1; i <= number; i++)
            {
                token.ThrowIfCancellationRequested();

                if (Util.IsPrime(i))
                {
                    counter++;
                }

                if (number < 100)
                {
                    progress.Report((int)(i * 100 / number));
                }
                // высокая частота обновления шкалы прогресса не нужна,
                // (во-первых, значения в ней от 0 до 100, а, во-вторых, частое обновление сильно тормозит отклик в GUI)
                else if (i % (number / 100) == 0)
                {
                    progress.Report((int)(i * 100 / number));
                }
            }
            progress.Report(100);

            return counter;
        }

        private TextBox CreateNumberTextBox() => new TextBox
        {
            Anchor = AnchorStyles.None,
            Location = new System.Drawing.Point(0, 0),
            Name = "numberTextBox",
            Size = new System.Drawing.Size(100, 22)
        };

        private Button CreateRunButton() => new Button
        {
            Anchor = AnchorStyles.None,
            Location = new System.Drawing.Point(0, 0),
            Name = "runButton",
            Size = new System.Drawing.Size(75, 23),
            Text = "Run",
            UseVisualStyleBackColor = true
        };

        private Button CreateCancelButton() => new Button
        {
            Anchor = AnchorStyles.None,
            Location = new System.Drawing.Point(0, 0),
            Name = "runButton",
            Size = new System.Drawing.Size(75, 23),
            Text = "Cancel",
            UseVisualStyleBackColor = true
        };

        private Label CreateStatusLabel() => new Label
        {
            Anchor = AnchorStyles.None,
            AutoSize = true,
            Location = new System.Drawing.Point(0, 0),
            Name = "statusLabel",
            Size = new System.Drawing.Size(20, 17),
            Text = "...",
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        };

        private ProgressBar CreateProgressBar() => new ProgressBar
        {
            Anchor = AnchorStyles.None,
            Location = new System.Drawing.Point(0, 0),
            Name = "progressBar",
            Size = new System.Drawing.Size(100, 23),
            Maximum = 100,
            Step = 1

        };

        private Label CreateResultLabel() => new Label
        {
            Anchor = AnchorStyles.None,
            AutoSize = true,
            Location = new System.Drawing.Point(0, 0),
            Name = "resultLabel",
            Size = new System.Drawing.Size(20, 17),
            Text = "...",
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        };
    }
}
