using System;
using OptionalUI;
using RWCustom;
using UnityEngine;

namespace MoreOverseers
{
    class ConfigMenu : OptionInterface
    {
        public ConfigMenu() : base(plugin: OverseersPlugin.Instance) { }

        public override void Initialize()
        {
            base.Initialize();
            Tabs = new OpTab[1];
            Tabs[0] = new OpTab("Overseers");

            cModeBox = new OpComboBox(new Vector2(200, 210), 150, "colourMode", Enum.GetNames(typeof(ColourMode)), "RandomStatic");
            cPickerLabel = new OpLabel(150, 160, "Custom colour selector");
            cPicker = new OpColorPicker(new Vector2(200, 7), "customColour", "A369E0");

            Tabs[0].AddItems(
                new OpLabel(new Vector2(100, 550), new Vector2(400, 40), rwMod.ModID, bigText: true),
                new OpLabel(new Vector2(150, 500), new Vector2(150, 30), $"v{rwMod.Version}"),
                new OpLabel(new Vector2(300, 500), new Vector2(150, 30), $"by {rwMod.author}"),

                new OpLabel(150, 400, "Minimum overseer count per region"),
                new OpSlider(new Vector2(200, 360), "min", new IntVector2(5, 100), 200, false, defaultMin),

                new OpLabel(150, 320, "Maximum overseer count per region"),
                new OpSlider(new Vector2(200, 280), "max", new IntVector2(5, 100), 200, false, defaultMax),

                new OpLabel(150, 240, "Colour mode"),
                cModeBox,

                cPickerLabel, cPicker
            );
        }

        OpComboBox cModeBox;
        OpLabel cPickerLabel;
        OpColorPicker cPicker;

        const int defaultMin = 10;
        const int defaultMax = 25;

        public override void ConfigOnChange()
        {
            base.ConfigOnChange();

            MinOverseers = Mathf.Clamp(int.Parse(config["min"]), 0, 100);
            MaxOverseers = Mathf.Clamp(int.Parse(config["max"]), MinOverseers + 1, 101);

            ColourMode_ = (ColourMode)Enum.Parse(typeof(ColourMode), config["colourMode"]);

            string cStr = config["customColour"];
            CustomColour = new Color(
                    int.Parse(cStr.Substring(0, 2), System.Globalization.NumberStyles.HexNumber) / 360f,
                    int.Parse(cStr.Substring(2, 2), System.Globalization.NumberStyles.HexNumber) / 360f,
                    int.Parse(cStr.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) / 360f
                    );

            OverseersPlugin.Logger_.LogInfo($"custom overseer colour : {CustomColour}");

        }

        public override void Update(float dt)
        {
            base.Update(dt);

            if (cModeBox.value == "Custom")
            {
                cPickerLabel.Show();
                cPicker.Show();
            }
            else
            {
                cPickerLabel.Hide();
                cPicker.Hide();
            }
        }

        public static int MinOverseers { get; private set; }
        public static int MaxOverseers { get; private set; }
        public static ColourMode ColourMode_ { get; private set; }
        public static Color CustomColour { get; private set; }

        public enum ColourMode
        {
            Default,
            RandomStrobe,
            RandomStatic,
            AllPebbles,
            AllMoon,
            AllGreenOverseerFromUnknownIterator,
            Custom
        }

    }
}
