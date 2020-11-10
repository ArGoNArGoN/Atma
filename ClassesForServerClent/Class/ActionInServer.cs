namespace ClassesForServerClent.Class
{
	public enum ActionInServer
	{
		UserLoggedInToServer,       // Пользователь зашел на сервер
		UserLoggedOutOfServer,      // Пользователь покинул сервер
		UserSendMessage,            // Пользователь отправил сообщение
		UserDeleteMessage,          // Пользователь удалил сообщение
		UserGotRole,                // Пользователь получил Роль
		UserHasLostRole,            // Пользователь утратил Роль
		UserGotRight,				// Пользователь получил Право
		UserHasLostRight,           // Пользователь утратил Право
		ChatCreate,                 // Чат создан
		ChatUpdate,                 // Чат обнавлен
		ChatDeleted,                // Чат удален
		TextChatCreate,             // Текстовый чат создан
		TextChatUpdate,             // Текстовый чат обнавлен
		TextChatDeleted,            // Текстовый чат удален
		RoleCreate,					// Роль создана
		RoleUpdate,					// Роль изменена
		RoleDeleted,				// Роль удалена
		RightCreate,				// Право создано
		RightUdate,					// Право изменено
		RightDeleted,				// Право удалено
	}
}