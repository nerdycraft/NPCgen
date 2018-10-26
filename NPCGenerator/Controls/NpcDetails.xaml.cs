using NPCGenerator.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace NPCGenerator.Controls
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// </summary>
    public partial class NpcDetails
    {
        private readonly TextBlock[] attrs;
        private readonly TextBlock[] rolls;
        public NpcDetails()
        {
            InitializeComponent();
            attrs = new[] { attr1, attr2, attr3 };
            rolls = new[] { roll1, roll2, roll3 };
        }

        private void Roll_Click(object sender, RoutedEventArgs e)
        {
            if (!(((Button)sender).Tag is AttrTalent talent))
                throw new ArgumentNullException(nameof(sender));
            if (!(DataContext is NPC npc))
                throw new ArgumentNullException(nameof(sender));

            var fw = (int)talent.Value;
            int failCount = 0, successCount = 0;

            var values = new[] {talent.Attr1, talent.Attr2, talent.Attr3};

            rollInfo.Text = talent.Name;

            for (var roll = 0; roll < 3; roll++)
            {
                switch (Roll(attrs[roll], rolls[roll], npc, values[roll], ref fw))
                {
                case RollResult.Fail:
                    failCount++;
                    break;
                case RollResult.Success:
                    successCount++;
                    break;
                }
            }

            //Result
            if ( successCount >= 2 )
                rollResult.Text = "!!KRIT ERFOLG!!";
            else if ( failCount >= 2 )
                rollResult.Text = "!!PATZER!!";
            else if ( fw < 0 )
                rollResult.Text = "misslungen";
            else
                rollResult.Text = "Erfolg! QS = " + ( fw == 0 ? 1 : (int)Math.Ceiling( fw / 3f ) );
        }

        private readonly Random rnd = new Random();
        private RollResult Roll(TextBlock tbAttr, TextBlock tbRoll, NPC npc, string attr, ref int fw)
        {
            tbAttr.Text = attr;
            var roll = rnd.Next(1, 20);
            var value = (int)npc.Attributes.GetFromStr(attr);

            if (roll > value)
                fw -= roll - value;

            tbRoll.Text = roll.ToString();

            return roll == 20 ? RollResult.Fail : roll == 1 ? RollResult.Success : RollResult.Normal;
        }

        private enum RollResult
        {
            Normal,
            Fail,
            Success
        }
    }
}
