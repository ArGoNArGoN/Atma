namespace ClassesForServerClent.Class
{
	public enum ActionInServer
	{
		UserLoggedInToServer,       // 0  Пользователь зашел на сервер
		UserLoggedOutOfServer,      // 1  Пользователь покинул сервер
		UserSendMessage,            // 2  Пользователь отправил сообщение
		UserDeleteMessage,          // 3  Пользователь удалил сообщение
		UserGotRole,                // 4  Пользователь получил Роль
		UserHasLostRole,            // 5  Пользователь утратил Роль
		UserGotRight,				// 6  Пользователь получил Право
		UserHasLostRight,           // 7  Пользователь утратил Право
		ChatCreate,                 // 8  Чат создан
		ChatUpdate,                 // 9  Чат обнавлен
		ChatDeleted,                // 10 Чат удален
		TextChatCreate,             // 11 Текстовый чат создан
		TextChatUpdate,             // 12 Текстовый чат обнавлен
		TextChatDeleted,            // 13 Текстовый чат удален
		RoleCreate,					// 14 Роль создана
		RoleUpdate,					// 15 Роль изменена
		RoleDeleted,				// 16 Роль удалена
		RightCreate,				// 17 Право создано
		RightUdate,					// 18 Право изменено
		RightDeleted,               // 19 Право удалено
		ServerCreate,               // 20 Сервер создан
		ServerUpdate,               // 21 Сервер изменен
		ServerDeleted,              // 22 Сервер удален
		OpinionCreate,              // 23 Отзыв создан
		OpinionUpdate,              // 24 Отзыв изменен
		OpinionDeleted,             // 25 Отзыв удален
	}
}