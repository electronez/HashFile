namespace HashFiles
{
    public interface ISqlInsert
    {
        /// <summary>
        /// Вставка сообщений (хэш файла)
        /// </summary>
        /// <param name="message"></param>
        void InsertFileHash(EntityMessage message);

        /// <summary>
        /// Вставка ошибок
        /// </summary>
        /// <param name="exception"></param>
        void InsertException(EntityException exception);
    }
}
