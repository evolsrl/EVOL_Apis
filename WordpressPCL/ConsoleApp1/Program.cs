using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPressPCL;
using WordPressPCL.Models;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            CreatePost().Wait();
        }

        private static async Task CreatePost()
        {
            try
            {
                WordPressClient client = await GetClient();
                if (await client.IsValidJWToken())
                {
                    var post = new Post
                    {
                        Title = new Title("Nuevo post"),
                        Content = new Content("Content for new post.")
                    };
                    await client.Posts.Create(post);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
                Console.ReadKey();
            }
        }

        private static async Task<WordPressClient> GetClient()
        {
            // JWT authentication
            var client = new WordPressClient("http://localhost:8080/wordpress/wp-json");
            client.AuthMethod = AuthMethod.JWT;
            await client.RequestJWToken("admin", "1234");
            return client;
        }
    }
}
