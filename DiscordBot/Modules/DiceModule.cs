using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using System.Text.RegularExpressions;

namespace DiscordBot.Modules
{
    public class DiceModule : ModuleBase<SocketCommandContext>
    {
        [Command("roll")]
        [Summary("Roll dice with specified face, amount, and modifier.")]
        public async Task RollDiceAsync([Remainder] [Summary("The dice to roll")] string param)
        {
            int total = 0, numOfDice = 0, dieType = 0, mod, roll;
            string sTotal = null;
            Random random = new Random();
            string pattern = @"([\d]*)[d]([\d]*)([+|-]\d)*";
            Match match = Regex.Match(param, pattern);

            if (match.Groups[2].ToString() == "")
                await ReplyAsync("Please enter a valid die type.");
            else
                dieType = Convert.ToInt32(match.Groups[2].ToString());

            // Assume numOfDice is one if not specified
            if (match.Groups[1].ToString() == "")
                numOfDice = 1;
            else
                numOfDice = Convert.ToInt32(match.Groups[1].ToString());

            // Assume mod is zero if not specified
            if (match.Groups[3].ToString() == "")
                mod = 0;
            else
                mod = Convert.ToInt32(match.Groups[3].ToString());

            if (dieType == 20 && numOfDice < 3 && mod == 0)
            {
                for (; numOfDice > 0; numOfDice--)
                {
                    roll = random.Next(1, dieType);
                    sTotal += roll.ToString();

                    if (numOfDice > 1)
                        sTotal += ", ";
                }
            }
            else
            {
                for (; numOfDice > 0; numOfDice--)
                {
                    roll = random.Next(1, dieType);
                    sTotal += "**" + roll.ToString() + "**";
                    total += roll;

                    if (numOfDice > 1)
                        sTotal += " + ";
                }

                if (mod != 0)
                {
                    total += mod;
                    sTotal += " + " + mod.ToString();
                }

                sTotal += " = " + total.ToString();
            }
                
            await ReplyAsync(sTotal);
        }
    }
}
