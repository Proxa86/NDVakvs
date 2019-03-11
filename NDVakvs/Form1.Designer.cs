namespace NDVakvs
{
    partial class Form1
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
            this.buttonOpenFolderForInsertMarker = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonOpenFolderForInsertMarker
            // 
            this.buttonOpenFolderForInsertMarker.Location = new System.Drawing.Point(36, 34);
            this.buttonOpenFolderForInsertMarker.Name = "buttonOpenFolderForInsertMarker";
            this.buttonOpenFolderForInsertMarker.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenFolderForInsertMarker.TabIndex = 0;
            this.buttonOpenFolderForInsertMarker.Text = "OpenFolderForInsertMarker";
            this.buttonOpenFolderForInsertMarker.UseVisualStyleBackColor = true;
            this.buttonOpenFolderForInsertMarker.Click += new System.EventHandler(this.buttonOpenFolderForInsertMarker_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonOpenFolderForInsertMarker);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOpenFolderForInsertMarker;
    }
}

