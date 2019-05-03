namespace SimpleCS.Entity
{
    public class ServerHandler
    {
        /// <summary>
        /// Основной метод для обработки входящего запроса и генерации ответа (если нужно)
        /// </summary>
        /// <param name="Data">Запрос от клиента</param>
        /// <returns>Ответ клиенту (null - ответ не нужен)</returns>
        public virtual string ProcessRequest(string Data)
        {
            return Data;
        }
    }
}
