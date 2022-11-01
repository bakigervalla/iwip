using System;
using System.Collections.Generic;
using System.Text;

namespace iwip.Models
{
    public class Alert
    {
        public Alert(AlertType type, string message)
        {
            this.Type = type;
            this.Message = message;
        }

        public string Message { get; set; }
        public AlertType Type { get; set; }
    }

    public enum AlertType {
        success,
        info,
        danger,
        warning,
    }
}
