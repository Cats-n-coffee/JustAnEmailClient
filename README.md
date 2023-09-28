# JustAnEmailClient

This is an ongoing project. The main (or basic) functionality and UI are mostly done, but a lot of details and cases are not handled yet. The code will get cleaned and probably reorganized.
This current version only supports Outlook email accounts, see the Remaining items below for additional information.

## Architecture
The current app architecture is as follows:
```
LoadingPage: Are there creds on file?
  |-- Yes: EmailPage --> fetch folders and emails, add receive and new message buttons
              |-- NewMessagePage --> used to write new, reply and forward messages
  |-- No: LoginPage 
```
The `Login` and `Email` pages are both views in displayed in the main window. They each have a route registered in `AppShell`. The `NewMessage` view is displayed in a new window to follow the pattern most email clients use, and because it's easier to use than creating tabs. 
### New Message Window
The New Message view easily opens in a new window, but closing it and closing the correct window was more complicated. Following [the docs](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/windows#multi-window-support), the `CloseWindow` methods needs to take the newly instantiated `Window` object, which will require some adjustements, so it was added to the list of remaining items. In its current state, the opening of multiple windows and closing using the `Send` button will cause the wrong window to close (window at position 1).

## Send/Receive Emails
Using `MailKit` for SMTP and IMAP connections and operations (along with `MimeKit`).

## Remaning Items
- Error handling
- Form/inputs validation and error display
- Review credentials read/write and add error handling
- Store emails
- Close window in New Message using object
- Add support for Gmail
- Add support for multiple accounts/mail boxes in the same Email view
