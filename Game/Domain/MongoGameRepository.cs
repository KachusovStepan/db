using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace Game.Domain
{
    // TODO Сделать по аналогии с MongoUserRepository
    public class MongoGameRepository : IGameRepository
    {
        public const string CollectionName = "games";
        private readonly IMongoCollection<GameEntity> gameCollection;

        public MongoGameRepository(IMongoDatabase db)
        {
            gameCollection = db.GetCollection<GameEntity>(CollectionName);
        }

        public GameEntity Insert(GameEntity game)
        {
            gameCollection.InsertOne(game);
            return game;
        }

        public GameEntity FindById(Guid gameId)
        {
            return gameCollection.Find(g => g.Id == gameId).FirstOrDefault();
        }

        public void Update(GameEntity game)
        {
            gameCollection.ReplaceOne(g => g.Id == game.Id, game);
        }

        // Возвращает не более чем limit игр со статусом GameStatus.WaitingToStart
        public IList<GameEntity> FindWaitingToStart(int limit)
        {
            //TODO: Используй Find и Limit
            return gameCollection.Find(g => g.Status == GameStatus.WaitingToStart)
                .Limit(limit).ToList();
        }

        // Обновляет игру, если она находится в статусе GameStatus.WaitingToStart
        public bool TryUpdateWaitingToStart(GameEntity game)
        {
            //TODO: Для проверки успешности используй IsAcknowledged и ModifiedCount из результата
            var res = gameCollection.ReplaceOne(g => g.Status == GameStatus.WaitingToStart, game);
            return res.IsAcknowledged && res.ModifiedCount == 1;
        }
    }
}