using System;
using System.Linq;
using System.Threading.Tasks;
using HomeApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;
        
        public RoomRepository (HomeApiContext context)
        {
            _context = context;
        }
        
        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await _context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }
        
        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                await _context.Rooms.AddAsync(room);
            
            await _context.SaveChangesAsync();
        }

        /// <summary>
        ///  Изменить комнату
        /// </summary>
        public async Task ChangeRoomInfo(Room room)
        {
            var CastomizedRoom = await GetRoomByName(room.Name);
            if(CastomizedRoom == null)
            {
                throw new ArgumentException("Такой комнаты не найдено.");
            }

            //На корректность параметры проверяет валидатор, поэтому в этом методе я просто обновлю значения

            CastomizedRoom.Name = room.Name;
            CastomizedRoom.Area = room.Area;
            CastomizedRoom.Voltage = room.Voltage;
            CastomizedRoom.GasConnected = room.GasConnected;

            //По-хорошему при изменении параметров "Voltage" и "GasConnected" нужно пройтись по всем устройствам 
            //в этой комнате и проверить останется ли корректным их подключение.
            //Но не думаю что задание это подразумевало

            await _context.SaveChangesAsync();
        }
    }
}