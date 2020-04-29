using Discord;
using Discord.Net;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace SleepyBerry.Modules
{
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
    public class ExampleCommands : ModuleBase
    {
        // Lists all commands
        [Command("help")]
        [Summary("Shows all commands")]
        public async Task HelpCommand()
        {
            // initialize empty string builder for reply
            var sb = new StringBuilder();

            // get user info from the Context
            var user = Context.User;

            // list of available commands
            List<String> coms = new List<String>{
                "!ask [Question] - ask me a question!",
                "!help - get list of available commands",
                "!hello - say hi to me!",
                "!wth [City] - show the current weather of the city you entered",
                "!temp [City] - show only the current weather of the city you entered"
            };

            // build out the reply
            sb.AppendLine($"These are my available commands:");
            sb.AppendLine();
            for (int i = 0; i < coms.Count-1; i++)
            {
                sb.AppendLine(coms[i]);
            }

            // send simple string reply
            await ReplyAsync(sb.ToString());

        }

        // Respond with a greeting
        [Command("hello")]
        [Alias("hi", "sup")]
        public async Task HelloCommand()
        {
            // initialize empty string builder for reply
            var sb = new StringBuilder();

            // get user info from the Context
            var user = Context.User;

            // list of possible greetings
            List<String> greet = new List<String>{
                "Howdy",
                "Greetings",
                "Hello",
                "Hi",
                "Sup"
            };

            String ranGreet = greet[new Random().Next(greet.Count - 1)];

            // build out the reply
            sb.AppendLine(ranGreet + $", " + user.Username + "!");

            // send simple string reply
            await ReplyAsync(sb.ToString());
        }

        // Receive simple answer for question provided
        [Command("8ball")]
        [Alias("ask")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task AskEightBall([Remainder]string args = null)
        {
            // I like using StringBuilder to build out the reply
            var sb = new StringBuilder();
            // let's use an embed for this one!
            var embed = new EmbedBuilder();

            // get user info from the Context
            var user = Context.User;

            // now to create a list of possible replies
            var replies = new List<string>();

            // add our possible replies
            replies.Add("yes");
            replies.Add("no");
            replies.Add("maybe");
            replies.Add("hazzzzy....");
            //replies.Add("you crazy?!");
            //replies.Add("zzz...");

            // time to add some options to the embed (like color and title)
            embed.WithColor(new Color(0, 255, 0));
            embed.Title = "Welcome to the 8-ball!";

            // we can get lots of information from the Context that is passed into the commands
            // here I'm setting up the preface with the user's name and a comma
            sb.AppendLine(user + $",");
            sb.AppendLine();

            // let's make sure the supplied question isn't null 
            if (args == null)
            {
                // if no question is asked (args are null), reply with the below text
                sb.AppendLine("Sorry, can't answer a question you didn't ask!");
            }
            else
            {
                // if we have a question, let's give an answer!
                // get a random number to index our list with (arrays start at zero so we subtract 1 from the count)
                var answer = replies[new Random().Next(replies.Count - 1)];

                // build out our reply with the handy StringBuilder
                sb.AppendLine($"You asked: [" + args.ToString() + "]...");
                sb.AppendLine();
                sb.AppendLine($"...your answer is [" + answer.ToString() + "]");

                // bonus - let's switch out the reply and change the color based on it
                switch (answer)
                {
                    case "yes":
                        {
                            embed.WithColor(new Color(0, 255, 0));
                            break;
                        }
                    case "no":
                        {
                            embed.WithColor(new Color(255, 0, 0));
                            break;
                        }
                    case "maybe":
                        {
                            embed.WithColor(new Color(255, 255, 0));
                            break;
                        }
                    case "hazzzzy....":
                        {
                            embed.WithColor(new Color(255, 0, 255));
                            break;
                        }
                }
            }

            // now we can assign the description of the embed to the contents of the StringBuilder we created
            embed.Description = sb.ToString();

            // this will reply with the embed
            await ReplyAsync(null, false, embed.Build());
        }

        // !shithole -> Yeah it's a shithole
        [Command("shithole")]
        [Summary("L4D2 reference")]
        public async Task Shithole() {
            // initialize empty string builder for reply
            var sb = new StringBuilder();

            // build out the reply - a Left 4 Dead 2 reference ;)
            sb.AppendLine($"Yeah, it's a shithole.");

            // send simple string reply
            await ReplyAsync(sb.ToString());
        }
    }
}