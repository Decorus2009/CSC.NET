namespace PrimeNumbers
{
    partial class PrimeNumbersCounterForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.outerTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.innerTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.numberColumnLabel = new System.Windows.Forms.Label();
            this.runColumnLabel = new System.Windows.Forms.Label();
            this.cancelColumnLabel = new System.Windows.Forms.Label();
            this.statusColumnLabel = new System.Windows.Forms.Label();
            this.progressColumnLabel = new System.Windows.Forms.Label();
            this.resultColumnLabel = new System.Windows.Forms.Label();
            this.addNewTaskButton = new System.Windows.Forms.Button();
            this.outerTableLayoutPanel.SuspendLayout();
            this.innerTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // outerTableLayoutPanel
            // 
            this.outerTableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.outerTableLayoutPanel.ColumnCount = 1;
            this.outerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.outerTableLayoutPanel.Controls.Add(this.innerTableLayoutPanel, 0, 1);
            this.outerTableLayoutPanel.Controls.Add(this.addNewTaskButton, 0, 0);
            this.outerTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outerTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.outerTableLayoutPanel.Name = "outerTableLayoutPanel";
            this.outerTableLayoutPanel.RowCount = 2;
            this.outerTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.78947F));
            this.outerTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84.21053F));
            this.outerTableLayoutPanel.Size = new System.Drawing.Size(682, 253);
            this.outerTableLayoutPanel.TabIndex = 2;
            // 
            // innerTableLayoutPanel
            // 
            this.innerTableLayoutPanel.AutoScroll = true;
            this.innerTableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.innerTableLayoutPanel.ColumnCount = 6;
            this.innerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.innerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.innerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.innerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.innerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.innerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.innerTableLayoutPanel.Controls.Add(this.numberColumnLabel, 0, 0);
            this.innerTableLayoutPanel.Controls.Add(this.runColumnLabel, 1, 0);
            this.innerTableLayoutPanel.Controls.Add(this.cancelColumnLabel, 2, 0);
            this.innerTableLayoutPanel.Controls.Add(this.statusColumnLabel, 3, 0);
            this.innerTableLayoutPanel.Controls.Add(this.progressColumnLabel, 4, 0);
            this.innerTableLayoutPanel.Controls.Add(this.resultColumnLabel, 5, 0);
            this.innerTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.innerTableLayoutPanel.Location = new System.Drawing.Point(4, 44);
            this.innerTableLayoutPanel.Name = "innerTableLayoutPanel";
            this.innerTableLayoutPanel.RowCount = 1;
            this.innerTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.innerTableLayoutPanel.Size = new System.Drawing.Size(674, 205);
            this.innerTableLayoutPanel.TabIndex = 1;
            // 
            // numberColumnLabel
            // 
            this.numberColumnLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numberColumnLabel.AutoSize = true;
            this.numberColumnLabel.Location = new System.Drawing.Point(27, 94);
            this.numberColumnLabel.Name = "numberColumnLabel";
            this.numberColumnLabel.Size = new System.Drawing.Size(58, 17);
            this.numberColumnLabel.TabIndex = 0;
            this.numberColumnLabel.Text = "Number";
            this.numberColumnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // runColumnLabel
            // 
            this.runColumnLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.runColumnLabel.AutoSize = true;
            this.runColumnLabel.Location = new System.Drawing.Point(151, 94);
            this.runColumnLabel.Name = "runColumnLabel";
            this.runColumnLabel.Size = new System.Drawing.Size(34, 17);
            this.runColumnLabel.TabIndex = 1;
            this.runColumnLabel.Text = "Run";
            this.runColumnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cancelColumnLabel
            // 
            this.cancelColumnLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cancelColumnLabel.AutoSize = true;
            this.cancelColumnLabel.Location = new System.Drawing.Point(255, 94);
            this.cancelColumnLabel.Name = "cancelColumnLabel";
            this.cancelColumnLabel.Size = new System.Drawing.Size(51, 17);
            this.cancelColumnLabel.TabIndex = 2;
            this.cancelColumnLabel.Text = "Cancel";
            this.cancelColumnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusColumnLabel
            // 
            this.statusColumnLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.statusColumnLabel.AutoSize = true;
            this.statusColumnLabel.Location = new System.Drawing.Point(368, 94);
            this.statusColumnLabel.Name = "statusColumnLabel";
            this.statusColumnLabel.Size = new System.Drawing.Size(48, 17);
            this.statusColumnLabel.TabIndex = 3;
            this.statusColumnLabel.Text = "Status";
            this.statusColumnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressColumnLabel
            // 
            this.progressColumnLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.progressColumnLabel.AutoSize = true;
            this.progressColumnLabel.Location = new System.Drawing.Point(472, 94);
            this.progressColumnLabel.Name = "progressColumnLabel";
            this.progressColumnLabel.Size = new System.Drawing.Size(65, 17);
            this.progressColumnLabel.TabIndex = 4;
            this.progressColumnLabel.Text = "Progress";
            this.progressColumnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // resultColumnLabel
            // 
            this.resultColumnLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.resultColumnLabel.AutoSize = true;
            this.resultColumnLabel.Location = new System.Drawing.Point(593, 94);
            this.resultColumnLabel.Name = "resultColumnLabel";
            this.resultColumnLabel.Size = new System.Drawing.Size(48, 17);
            this.resultColumnLabel.TabIndex = 5;
            this.resultColumnLabel.Text = "Result";
            this.resultColumnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // addNewTaskButton
            // 
            this.addNewTaskButton.Location = new System.Drawing.Point(4, 4);
            this.addNewTaskButton.Name = "addNewTaskButton";
            this.addNewTaskButton.Size = new System.Drawing.Size(149, 23);
            this.addNewTaskButton.TabIndex = 0;
            this.addNewTaskButton.Text = "Add New Task";
            this.addNewTaskButton.UseVisualStyleBackColor = true;
            this.addNewTaskButton.Click += new System.EventHandler(this.addNewTaskButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(682, 253);
            this.Controls.Add(this.outerTableLayoutPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.outerTableLayoutPanel.ResumeLayout(false);
            this.innerTableLayoutPanel.ResumeLayout(false);
            this.innerTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel outerTableLayoutPanel;
        private System.Windows.Forms.Button addNewTaskButton;
        private System.Windows.Forms.TableLayoutPanel innerTableLayoutPanel;
        private System.Windows.Forms.Label numberColumnLabel;
        private System.Windows.Forms.Label runColumnLabel;
        private System.Windows.Forms.Label cancelColumnLabel;
        private System.Windows.Forms.Label statusColumnLabel;
        private System.Windows.Forms.Label progressColumnLabel;
        private System.Windows.Forms.Label resultColumnLabel;
    }
}

