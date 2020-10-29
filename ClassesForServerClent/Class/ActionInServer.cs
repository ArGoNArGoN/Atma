namespace ClassesForServerClent.Class
{
	public enum ActionInServer
	{
		UserSendMessage,            // Пользователь отправил сообщение
		UserDeleteMessage,          // Пользователь удалил сообщение
		UserLoggedInToServer,       // Пользователь зашел на сервер
		UserLoggedOutOfServer,      // Пользователь покинул чат
		UserGotRole,                // Пользователь получил Роль
		UserHasLostRole,            // Пользователь утратил Роль
		UserGotRight,				// Пользователь получил Право
		UserHasLostRight,			// Пользователь утратил Право
		ChatDeleted,                // Чат удален
		ChatCreate,                 // Чат создан
		TextChatDeleted,            // Текстовый чат удален
		TextChatCreate,             // Текстовый чат создан
		CategoryDeleted,            // Категория удалена
		CategoryCreate,             // Категория создан
	}
}