using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Nemesis.Models;

namespace Nemesis.Network
{
    public class Database
    {
        public string DatabaseName { get; set; }
        private MongoClient client;
        public Database(string ConectionUrl)
        {
            client = new MongoClient(ConectionUrl);
        }
        public void InsertUser(string collection, User user)
        {
            client.GetDatabase(DatabaseName).GetCollection<User>(collection).InsertOne(user);
        }
        public async void InsertUserAync(string collection, User user)
        {
            await client.GetDatabase(DatabaseName).GetCollection<User>(collection).InsertOneAsync(user);
        }
        public async void RemoveUser(string collection, User user)
        {
            await client.GetDatabase(DatabaseName).GetCollection<User>(collection).DeleteOneAsync(
                Builders<User>.Filter.Eq(handle => handle.Name, user.Name));
        }
        public async Task<List<User>> GetUserList(string collection)
        {
            return await client.GetDatabase(DatabaseName).GetCollection<User>(collection).FindAsync(
                Builders<User>.Filter.Empty).Result.ToListAsync();
        }
        public async void ReplaceUser(string collection, User currentUserData, User newUserData)
        {
            await client.GetDatabase(DatabaseName).GetCollection<User>(collection).FindOneAndReplaceAsync(
                Builders<User>.Filter.Eq(handle => handle.Name, currentUserData.Name), newUserData);
        }
        public void ReplaceUser(string collection, string currentUserData, User newUserData)
        {
            ReplaceUser(collection, new User { Name = currentUserData }, newUserData);
        }
    }
}
