# Рабочее пространство Unity
> Используется студентами для работы с проектами Unity внутри колледжа и вне его.

### Установка
1. Перейдите в папку Assets своего VR-Проекта ([Как создать VR проект?](#Создание-VR-проекта))
2. Откройте её с помощью консоли. Как это сделать?
   * Правой кнопкой мыши внутри папки и выбор из контекстного меню поля "Open GIT Bush here"
      
      <img width="152" height="22" alt="image" src="https://github.com/user-attachments/assets/290eb2c9-3f5d-4ea6-8ad0-791f3c1f3d85" />

      *ИЛИ*
      
   * Вписать в строку пути проводника "cmd"
      
      <img width="207" height="37" alt="image" src="https://github.com/user-attachments/assets/7fd7088e-39e1-4226-91d7-8a9b44b78eb7" />
      
      <img width="223" height="53" alt="image" src="https://github.com/user-attachments/assets/fe55f19a-4045-40b2-91f8-95edbe4feb0f" />

3. Установка базовых настроек
    ```
    git config --global user.name "John Doe"
    git config --global user.email johndoe@example.com
    ```
    Напишите следующие команды, заменив данные на свои

4. Создайте Fork (Ответвление) от текущего репозитория ([Нажми на меня](https://github.com/SHUSTRIK-Milan/unity_workspace/fork))
5. Клонируем репозиторий
    ```
    git init
    git branch -M main
    git add remote origin "https://github.com/ссылка-на-ваш-репозиторий.git"
    ```
    Напишите следующие команды, заменив данные на свои
6. Создаем вашу рабочую папку
    ```
    mkdir "ваш логин от учётной записи" (Например: is112shust, pr112dedovec)
    ```
7. Переходим в эту папку
    ```
    cd "ваш логин от учётной записи"
    ```
    Работать в Unity вы будете именно в ней!

### Работа
Для фиксации ваших изменений используйте следующее:
1. Фиксация `git add .`
2. Коммит (Записка об изменениях) `git commit -m "Название записки" (Например: "добавил машинку")`
3. Отправка в удаленный репозиторий `git push`

### Создание VR проекта
