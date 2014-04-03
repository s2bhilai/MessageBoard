using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;

namespace MvcApp_MessageBoard.Data
{
    public class MessageBoardMigrationConfigurations
        :DbMigrationsConfiguration<MessageBoardContext>
    {
        public MessageBoardMigrationConfigurations()
        {
            this.AutomaticMigrationDataLossAllowed = true;
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MessageBoardContext context)
        {
            base.Seed(context);
#if DEBUG
            if (context.Topics.Count() == 0)
            {
                var topic = new Topic()
                {
                    Title="I Like ASP.Net MVC",
                    Body="It is making things easier",
                    Created=DateTime.UtcNow,
                    Replies=new List<Reply>()
                    {
                        new Reply()
                        {
                            Body="I love it too",
                            Created=DateTime.Now
                        },
                        new Reply()
                        {
                            Body="Me too",
                            Created=DateTime.Now
                        },
                        new Reply()
                        {
                            Body="Shucks",
                            Created=DateTime.Now
                        },
                    }
                };

                context.Topics.Add(topic);

                var anotherTopic=new Topic()
                {
                    Title="Ilike Ruby",
                    Created=DateTime.Now,
                    Body="Ruby on Rails is popular"
                };

                context.Topics.Add(anotherTopic);

                try
                {
                    context.SaveChanges();
                }
                catch(Exception ex)
                {
                    var msg=ex.Message;
                }
            }
#endif
        }
    }
}
