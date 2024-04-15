# EmailSenderProgram

This is a fictitious console application for sending emails to customers. It runs daily and supports multiple types of emails.

## Task 1

This was my thought-process whiles refactoring the code:
- First thing I did was to update the variable and method names to be more meaningful and self explanatory. That made the code cleaner and much more readable.
- Then I abstracted the fetching of the customers list into the main method. I did this because a request was being made to the data store twice to fetch the customers list, once in the "welcome" email method handler, and again in the "come back" email method.
- I also abstracted the actual email-sending process into a single method and reused it.
- This is a minor one, but I also cleared out most of the comments. I believe that well written code shouldn't need too many comments, if any.


## Task 2

If I wrote a similar program from scratch, assuming it was going to be used in the real world, these are some of the design and architechtural decisions I'd make:
- I'd break the code up into separate modules or classes, making sure each module is responsible for a single thing. This would make the code easier to maintain and scale. For example, there'd be a module to determine the type of email, a module to handle scheduling, a module to handle email template generations, etc.
- I'd use interfaces to abtstract some common functionality like sending email, and use dependency injection to handle the dependencies. This would make the code loosely coupled, and make it easier to swap out different email sending functionalities based on different conditions.
- I'd definitely use a background job system, like Quartz.Net, to run the email sending tasks.
- I'd use a json file to store configuration settings such as the SMTP server details. This would make it easier to change these config data without changing the code.
- I'd use a database for data storage. Which type of database? Well, that'll depend on the requirements of the project.
- I'd use a logger such as log4net to help track issues when the program is live.