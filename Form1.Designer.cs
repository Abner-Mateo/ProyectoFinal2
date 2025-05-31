namespace ProyectoFinal2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtProblema = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.btnCentroSalud = new System.Windows.Forms.Button();
            this.lblTermino = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtProblema
            // 
            this.txtProblema.Location = new System.Drawing.Point(18, 69);
            this.txtProblema.Name = "txtProblema";
            this.txtProblema.Size = new System.Drawing.Size(177, 20);
            this.txtProblema.TabIndex = 0;
            this.txtProblema.TextChanged += new System.EventHandler(this.txtProblema_TextChanged);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuscar.Location = new System.Drawing.Point(228, 63);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(108, 26);
            this.btnBuscar.TabIndex = 1;
            this.btnBuscar.Text = "\"Buscar\"";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // txtResultado
            // 
            this.txtResultado.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResultado.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtResultado.Location = new System.Drawing.Point(18, 95);
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.ReadOnly = true;
            this.txtResultado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultado.Size = new System.Drawing.Size(593, 343);
            this.txtResultado.TabIndex = 2;
            // 
            // btnCentroSalud
            // 
            this.btnCentroSalud.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCentroSalud.Location = new System.Drawing.Point(498, 50);
            this.btnCentroSalud.Name = "btnCentroSalud";
            this.btnCentroSalud.Size = new System.Drawing.Size(103, 39);
            this.btnCentroSalud.TabIndex = 3;
            this.btnCentroSalud.Text = "\"Buscar Centro Salud\"";
            this.btnCentroSalud.UseVisualStyleBackColor = true;
            this.btnCentroSalud.Click += new System.EventHandler(this.btnCentroSalud_Click);
            // 
            // lblTermino
            // 
            this.lblTermino.AutoSize = true;
            this.lblTermino.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTermino.Location = new System.Drawing.Point(33, 50);
            this.lblTermino.Name = "lblTermino";
            this.lblTermino.Size = new System.Drawing.Size(150, 16);
            this.lblTermino.TabIndex = 4;
            this.lblTermino.Text = "\"Consultas Medicas\"";
            this.lblTermino.Click += new System.EventHandler(this.lblTermino_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bernard MT Condensed", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(30, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(339, 31);
            this.label1.TabIndex = 5;
            this.label1.Text = "\"ASISTENTE MEDICO JUTIAPA\"";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::ProyectoFinal2.Properties.Resources.images;
            this.ClientSize = new System.Drawing.Size(773, 439);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTermino);
            this.Controls.Add(this.btnCentroSalud);
            this.Controls.Add(this.txtResultado);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.txtProblema);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtProblema;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.Button btnCentroSalud;
        private System.Windows.Forms.Label lblTermino;
        private System.Windows.Forms.Label label1;
    }
}