using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace EmailSenderProgram
{
    internal class Program
    {
        /// <summary>
        /// This application is run everyday
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            // Fetch customers from data store once, use multiple times
            List<Customer> customers = DataLayer.ListCustomers();

            Console.WriteLine("Send Welcomemail");
            bool areAllEmailsSent = SendWelcomeEmailWorker(customers);

#if DEBUG
            Console.WriteLine("Send Comebackmail");
            areAllEmailsSent = SendComebackEmailWorker(customers, "EOComebackToUs");
#else
            //Every Sunday run Comeback mail
            if (DateTime.Now.DayOfWeek.Equals(DayOfWeek.Sunday))
            {
                Console.WriteLine("Send Comebackmail");
                areAllEmailsSent = SendComebackEmailWorker(customers, "EOComebackToUs");
            }
#endif

            if (areAllEmailsSent)
            {
                Console.WriteLine("All mails are sent, I hope...");
            } else
            {
                Console.WriteLine("Oops, something went wrong when sending mail (I think...)");
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Send Welcome mail
        /// </summary>
        /// <returns></returns>
        public static bool SendWelcomeEmailWorker(List<Customer> customers)
        {
            try
            {
                foreach (Customer customer in customers)
                {
                    //If the customer is newly registered, one day back in time
                    if (customer.CreatedDateTime > DateTime.Now.AddDays(-1))
                    {
                        const string from = "info@EO.com";
                        string to = customer.Email;
                        const string subject = "Welcome as a new customer at EO!";
                        string body = "Hi " + customer.Email +
                                 "<br>We would like to welcome you as customer on our site!<br><br>Best Regards,<br>EO Team";
#if DEBUG
                        //Don't send mails in debug mode, just write the emails in console
                        Console.WriteLine("Send mail to:" + customer.Email);
#else
                        return SendEmail(from, to, subject, body);
#endif
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool SendComebackEmailWorker(List<Customer> customers, string voucher)
        {
            try
            {
                List<Order> orders = DataLayer.ListOrders();

                foreach (Customer customer in customers)
                {
                    // We send mail if customer hasn't put an order
                    bool send = true;
                    //loop through list of orders to see if customer don't exist in that list
                    foreach (Order order in orders)
                    {
                        // Email exists in order list
                        if (customer.Email == order.CustomerEmail)
                        {
                            //We don't send email to that customer
                            send = false;
                        }
                    }

                    //Send if customer hasn't put order
                    if (send == true)
                    {
                        const string from = "info@EO.com";
                        string to = customer.Email;
                        const string subject = "We miss you as a customer!";
                        var body = "Hi " + customer.Email +
                                 "<br>We miss you as a customer. Our shop is filled with nice products. Here is a voucher that gives you 50 kr to shop for." +
                                 "<br>Voucher: " + voucher +
                                 "<br><br>Best Regards,<br>EO Team";
#if DEBUG
                        //Don't send mails in debug mode, just write the emails in console
                        Console.WriteLine("Send mail to:" + customer.Email);
#else
                        return SendEmail(from, to, subject, body);
#endif
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool SendEmail(string from, string to, string subject, string body)
        {
            try
            {
                var message = new MailMessage(from, to, subject, body);

                var client = new SmtpClient("smtp.freesmtpservers.com");

                client.Port = 25;
                client.EnableSsl = true;

                client.Send(message);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Email sending failed: {e.Message}");
                return false;
            }
        }
    }
}