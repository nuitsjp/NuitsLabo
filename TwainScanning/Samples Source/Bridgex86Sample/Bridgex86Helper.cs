using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TwainScanning;
using TwainScanning.Bridgex86;

namespace Bridgex86Sample
{
    public static class Bridgex86Helper
    {

        #region Fill ComboBoxes with values

        private const string COMBOBOX_NOT_SUPPORTED_VALUE = "Not supported";

        public static void FillComboBox(ComboBox c, bool isSupportedCapability, ResultStringArray supportedValues, string currentValue)
        {
            c.Items.Clear();
            c.Enabled = true;

            if (!isSupportedCapability ||supportedValues.Status != StatusType.OK || string.IsNullOrEmpty(currentValue))
            {
                c.Enabled = false;
                c.Items.Add(COMBOBOX_NOT_SUPPORTED_VALUE);
                c.SelectedIndex = 0;
            }
            else
            {
                c.Items.AddRange(supportedValues.Value);
                c.SelectedItem = currentValue;
            }
        }

        public static void FillComboBox(ComboBox c, bool isSupportedCapability, ResultBool supportedValues, bool? currentValue)
        {
            c.Items.Clear();
            c.Enabled = true;

            if (!isSupportedCapability || supportedValues.Status != StatusType.OK || !currentValue.HasValue)
            {
                c.Enabled = false;
                c.Items.Add(COMBOBOX_NOT_SUPPORTED_VALUE);
                c.SelectedIndex = 0;
            }
            else
            {
                c.Items.Add(true.ToString());
                c.Items.Add(false.ToString());
                c.SelectedItem = currentValue.ToString();
            }
        }

        public static void FillComboBox(ComboBox c, bool isSupportedCapability, ResultStringArray supportedValues, ScanResolution? currentValue)
        {
            c.Items.Clear();
            c.Enabled = true;

            if (!isSupportedCapability || supportedValues.Status != StatusType.OK || !currentValue.HasValue)
            {
                c.Enabled = false;
                c.Items.Add(COMBOBOX_NOT_SUPPORTED_VALUE);
                c.SelectedIndex = 0;
            }
            else
            {
                c.Items.AddRange(supportedValues.Value);
                c.SelectedItem = currentValue.Value.X.ToString();
            }
        }

        public static void FillComboBox(ComboBox c, bool isSupportedCapability, string[] supportedValues, string currentValue)
        {
            c.Items.Clear();
            c.Enabled = true;

            if (!isSupportedCapability || supportedValues.Length == 0)
            {
                c.Enabled = false;
                c.Items.Add(COMBOBOX_NOT_SUPPORTED_VALUE);
                c.SelectedIndex = 0;
            }
            else
            {
                c.Items.AddRange(supportedValues);
                c.SelectedItem = currentValue;
            }
        }

        public static void FillComboBox(ComboBox c, bool isSupportedCapability, int supportedMin, int supportedMax, int currentValue)
        {
            c.Items.Clear();
            c.Enabled = true;

            if (!isSupportedCapability)
            {
                c.Enabled = false;
                c.Items.Add(COMBOBOX_NOT_SUPPORTED_VALUE);
                c.SelectedIndex = 0;
            }
            else
            {
                for (int i = supportedMin; i <= supportedMax; i++)
                {
                    c.Items.Add(i);
                }
                c.SelectedItem = currentValue;
            }
        }

        #endregion


        #region Get values for ScanSettings
        public static string GetValueAsString(ComboBox c)
        {
            if (!c.Enabled)
            {
                return null;
            }
            if (c.SelectedIndex < 0)
            {
                return null;
            }
            string val = c.SelectedItem.ToString();

            return val;
        }


        public static bool? GetValueAsBool(ComboBox c)
        {
            if (!c.Enabled)
            {
                return null;
            }
            if (c.SelectedIndex < 0)
            {
                return null;
            }
            string val = c.SelectedItem.ToString();

            var result = bool.Parse(val);
            return result;
        }

        public static bool GetValueAsBool(CheckBox c)
        {
            var result = c.Checked;
            return result;
        }

        public static float? GetValueAsFloat(ComboBox c)
        {
            if (!c.Enabled)
            {
                return null;
            }
            if (c.SelectedIndex < 0)
            {
                return null;
            }
            string val = c.SelectedItem.ToString();

            var result = float.Parse(val);
            return result;
        }

        public static ScanResolution? GetValueAsScanResolution(ComboBox c)
        {
            if (!c.Enabled)
            {
                return null;
            }
            if (c.SelectedIndex < 0)
            {
                return null;
            }
            float val = float.Parse(c.SelectedItem.ToString());

            var result = new ScanResolution(val);
            return result;
        }

        public static int? GetValueAsInt(ComboBox c)
        {
            if (!c.Enabled)
            {
                return null;
            }
            if (c.SelectedIndex < 0)
            {
                return null;
            }
            string val = c.SelectedItem.ToString();

            var result = int.Parse(val);
            return result;
        }

        public static short? GetValueAsShort(ComboBox c)
        {
            if (!c.Enabled)
            {
                return null;
            }
            if (c.SelectedIndex < 0)
            {
                return null;
            }
            string val = c.SelectedItem.ToString();

            var result = short.Parse(val);
            return result;
        }

        public static TEnum? GetValueAsEnum<TEnum>(ComboBox c) where TEnum : struct, Enum
        {
            if (!c.Enabled)
            {
                return null;
            }
            if (c.SelectedIndex < 0)
            {
                return null;
            }
            string val = c.SelectedItem.ToString();

            var result = (TEnum)Enum.Parse(typeof(TEnum), val, true);
            return result;
        }

        public static TEnum GetValueAsEnum<TEnum>(ComboBox c, TEnum defaultValue) where TEnum : struct, Enum
        {
            var result = GetValueAsEnum<TEnum>(c);
            if (!result.HasValue)
                return defaultValue;
            else
                return result.Value;
        }

        #endregion


        #region Enum helper methods
        public static string[] GetNamesFromEnum<TEnum>()
        {
            var enums = Enum.GetNames(typeof(TEnum));
            return enums;
        }

        #endregion


        #region Showing Bridgex86 messages
        public static void ShowBridgex86Messages(ResultBase result)
        {
            var caption = "Bridgex86 Messages";
            var status = "Status: " + result.Status;
            var icon = MessageBoxIcon.Information;
            var messages = string.Empty;
            var newLine = Environment.NewLine;

            if (!string.IsNullOrEmpty(result.WarningMessages))
            {
                messages += string.Format("{0}Warnings:{1}{2}", newLine, newLine, result.WarningMessages);
                icon = MessageBoxIcon.Warning;
            }
            if (!string.IsNullOrEmpty(result.ErrorMessages))
            {
                messages += string.Format("{0}Warnings:{1}{2}", newLine, newLine, result.ErrorMessages);
                icon = MessageBoxIcon.Error;
            }

            var text = string.Format("{0}{1}{2}", status, newLine, messages);
            MessageBox.Show(text, caption, MessageBoxButtons.OK, icon);
        }

        #endregion

    }
}
