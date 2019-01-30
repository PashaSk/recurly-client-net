/**********************************************************************
* Copyright (c) 1999-2014 Infratel, Inc.
* All rights reserved.
* http://www.infratel.com
*
***********************************************************************/

using System;
using System.Xml;
using System.IO;
using System.Diagnostics;
using Infratel.Utils.Text;

namespace Infratel.RecurlyLibrary
{
    public class RecurlyNotifications
    {
        /// <summary>
        /// This is the only Method that should be called externally.
        /// Please note: it may throw Exceptions
        /// </summary>
        public void ProcessNotification(byte[] body)
        {
            using (var memoryStream = new MemoryStream(body))
            {
                using (var streamReader = new StreamReader(memoryStream, System.Text.Encoding.UTF8))
                {
                    Trace.TraceInformation("{0}: ProcessNotification received a new message:\n{1}\n", DateTime.UtcNow.ToLog(),
                                           streamReader.ReadToEnd());
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    using (XmlTextReader xmlReader = new XmlTextReader(streamReader))
                    {
                        ReadXml(xmlReader);
                    }
                }
            }
        }

        /// <summary>
        /// Sent when a new account is created
        /// </summary>
        /// <param name="account"></param>
        protected virtual void NewAccountNotification(WebHookAccount account)
        {
            Trace.TraceInformation("{0}: {1}\n{2}\n", DateTime.UtcNow.ToLog(), "NewAccountNotification", account.ToString());
        }

        /// <summary>
        /// Sent when an account is closed. If an account is reopened, a new_account_notification is sent.
        /// </summary>
        /// <param name="account"></param>
        protected virtual void CanceledAccountNotification(WebHookAccount account)
        {
            Trace.TraceInformation("{0}: {1}\n{2}\n", DateTime.UtcNow.ToLog(), "CanceledAccountNotification", account.ToString());
        }

        /// <summary>
        /// Sent when billing information is successfully created or updated on an account.
        /// </summary>
        /// <param name="account"></param>
        protected virtual void BillingInfoUpdatedNotification(WebHookAccount account)
        {
            Trace.TraceInformation("{0}: {1}\n{2}\n", DateTime.UtcNow.ToLog(), "BillingInfoUpdatedNotification", account.ToString());
        }

        /// <summary>
        /// Sent when an account subscription is reactivated after having been canceled.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="subscription"></param>
        protected virtual void ReactivatedAccountNotification(WebHookAccount account, WebHookSubscription subscription)
        {
            Trace.TraceInformation("{0}: {1}\n{2}\n{3}\n", DateTime.UtcNow.ToLog(), "ReactivatedAccountNotification", account.ToString(), subscription.ToString());
        }

        /// <summary>
        /// Sent when a new subscription is created.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="subscription"></param>
        protected virtual void NewSubscriptionNotification(WebHookAccount account, WebHookSubscription subscription)
        {
            Trace.TraceInformation("{0}: {1}\n{2}\n{3}\n", DateTime.UtcNow.ToLog(), "NewSubscriptionNotification", account.ToString(), subscription.ToString());
        }

        /// <summary>
        /// When a subscription is upgraded or downgraded, Recurly will send an updated_subscription_notification. 
        /// The notification is sent after the modification is performed. If you modify a subscription and it takes
        /// place immediately, the notification will also be sent immediately. If the subscription change takes effect 
        /// at renewal, then the notification will be sent when the subscription renews. Therefore, if you receive an 
        /// updated_subscription_notification, it contains the latest subscription information.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="subscription"></param>
        protected virtual void UpdatedSubscriptionNotification(WebHookAccount account, WebHookSubscription subscription)
        {
            Trace.TraceInformation("{0}: {1}\n{2}\n{3}\n", DateTime.UtcNow.ToLog(), "UpdatedSubscriptionNotification", account.ToString(), subscription.ToString());
        }

        /// <summary>
        /// The canceled_subscription_notification is sent when a subscription is canceled. This means the 
        /// subscription will not renew. The subscription state is set to canceled but the subscription is still 
        /// valid until the expires_at date. The next notification is sent when the subscription is completely terminated.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="subscription"></param>
        protected virtual void CanceledSubscriptionNotification(WebHookAccount account, WebHookSubscription subscription)
        {
            Trace.TraceInformation("{0}: {1}\n{2}\n{3}\n", DateTime.UtcNow.ToLog(), "CanceledSubscriptionNotification", account.ToString(), subscription.ToString());
        }

        /// <summary>
        /// The expired_subscription_notification is sent when a subscription is no longer valid. This can happen if 
        /// a canceled subscription expires or if an active subscription is refunded (and terminated immediately). 
        /// If you receive this message, the account no longer has a subscription.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="subscription"></param>
        protected virtual void ExpiredSubscriptionNotification(WebHookAccount account, WebHookSubscription subscription)
        {
            Trace.TraceInformation("{0}: {1}\n{2}\n{3}\n", DateTime.UtcNow.ToLog(), "ExpiredSubscriptionNotification", account.ToString(), subscription.ToString());
        }

        /// <summary>
        /// The renewed_subscription_notification is sent whenever a subscription renews. This notification is sent 
        /// regardless of a successful payment being applied to the subscription—it indicates the previous term is over 
        /// and the subscription is now in a new term. If you are performing metered or usage-based billing, use this 
        /// notification to reset your usage stats for the current billing term.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="subscription"></param>
        protected virtual void RenewedSubscriptionNotification(WebHookAccount account, WebHookSubscription subscription)
        {
            Trace.TraceInformation("{0}: {1}\n{2}\n{3}\n", DateTime.UtcNow.ToLog(), "RenewedSubscriptionNotification", account.ToString(), subscription.ToString());
        }

        /// <summary>
        /// A successful_payment_notification is sent when a payment is successfully captured.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="transaction"></param>
        protected virtual void SuccessfulPaymentNotification(WebHookAccount account, WebHookTransaction transaction)
        {
            Trace.TraceInformation("{0}: {1}\n{2}\n{3}\n", DateTime.UtcNow.ToLog(), "SuccessfulPaymentNotification", account.ToString(), transaction.ToString());
        }

        /// <summary>
        /// A failed_payment_notification is sent when a payment attempt is declined by the payment gateway.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="transaction"></param>
        protected virtual void FailedPaymentNotification(WebHookAccount account, WebHookTransaction transaction)
        {
            Trace.TraceInformation("{0}: {1}\n{2}\n{3}\n", DateTime.UtcNow.ToLog(), "FailedPaymentNotification", account.ToString(), transaction.ToString());
        }

        /// <summary>
        /// f you refund an amount through the API or admin interface, a successful_refund_notification is sent. 
        /// Failed refund attempts do not generate a notification.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="transaction"></param>
        protected virtual void SuccessfulRefundNotification(WebHookAccount account, WebHookTransaction transaction)
        {
            Trace.TraceInformation("{0}: {1}\n{2}\n{3}\n", DateTime.UtcNow.ToLog(), "SuccessfulRefundNotification", account.ToString(), transaction.ToString());
        }

        /// <summary>
        /// If you void a successfully captured payment before it settles, a void_payment_notification is sent. 
        /// Payments can only be voided before the funds settle into your merchant account.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="transaction"></param>
        protected virtual void VoidPaymentNotification(WebHookAccount account, WebHookTransaction transaction)
        {
            Trace.TraceInformation("{0}: {1}\n{2}\n{3}\n", DateTime.UtcNow.ToLog(), "VoidPaymentNotification", account.ToString(), transaction.ToString());
        }

        /// <summary>
        /// If a new invoice is generated, a new_invoice_notification is sent.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="invoice"></param>
        protected virtual void NewInvoiceNotification(WebHookAccount account, WebHookInvoice invoice)
        {
            Trace.TraceInformation("{0}: {1}\n{2}\n{3}\n", DateTime.UtcNow.ToLog(), "NewInvoiceNotification", account.ToString(), invoice.ToString());
        }

        /// <summary>
        /// If an invoice is closed, a closed_invoice_notification is sent. A closed invoice can result from 
        /// either a failed to collect invoice or fully paid invoice.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="invoice"></param>
        protected virtual void ClosedInvoiceNotification(WebHookAccount account, WebHookInvoice invoice)
        {
            Trace.TraceInformation("{0}: {1}\n{2}\n{3}\n", DateTime.UtcNow.ToLog(), "ClosedInvoiceNotification", account.ToString(), invoice.ToString());
        }

        /// <summary>
        /// If an invoice is past due, a past_due_invoice_notification is sent. An invoice is past due when can 
        /// result from either a failed to collect invoice or fully paid invoice.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="invoice"></param>
        protected virtual void PastDueInvoiceNotification(WebHookAccount account, WebHookInvoice invoice)
        {
            Trace.TraceInformation("{0}: {1}\n{2}\n{3}\n", DateTime.UtcNow.ToLog(), "PastDueInvoiceNotification", account.ToString(), invoice.ToString());
        }
        /// <summary>
        /// If transaction was declined in Kount by hand, Recurly sends webhook about decision. 
        /// We should check the actual status via API call and than close account in case of a fraud
        /// </summary>
        /// <param name="account"></param>
        /// <param name="invoice"></param>
        protected virtual void FraudInfoUpdatedNotification(WebHookAccount account, WebHookTransaction transaction)
        {
            Trace.TraceInformation("{0}: {1}\n{2}\n{3}\n", DateTime.UtcNow.ToLog(), "FraudInfoUpdatedNotification", account.ToString(), transaction.ToString());
        }

        private void ReadXml(XmlTextReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "new_account_notification":
                            NewAccountNotification(new WebHookAccount(reader));
                            return;
                        case "canceled_account_notification":
                            CanceledAccountNotification(new WebHookAccount(reader));
                            return;
                        case "billing_info_updated_notification":
                            BillingInfoUpdatedNotification(new WebHookAccount(reader));
                            return;
                        case "reactivated_account_notification":
                            ReactivatedAccountNotification(new WebHookAccount(reader), new WebHookSubscription(reader));
                            return;

                        case "new_subscription_notification":
                            NewSubscriptionNotification(new WebHookAccount(reader), new WebHookSubscription(reader));
                            return;
                        case "updated_subscription_notification":
                            UpdatedSubscriptionNotification(new WebHookAccount(reader), new WebHookSubscription(reader));
                            return;
                        case "canceled_subscription_notification":
                            CanceledSubscriptionNotification(new WebHookAccount(reader), new WebHookSubscription(reader));
                            return;
                        case "expired_subscription_notification":
                            ExpiredSubscriptionNotification(new WebHookAccount(reader), new WebHookSubscription(reader));
                            return;
                        case "renewed_subscription_notification":
                            RenewedSubscriptionNotification(new WebHookAccount(reader), new WebHookSubscription(reader));
                            return;

                        case "successful_payment_notification":
                            SuccessfulPaymentNotification(new WebHookAccount(reader), new WebHookTransaction(reader));
                            return;
                        case "failed_payment_notification":
                            FailedPaymentNotification(new WebHookAccount(reader), new WebHookTransaction(reader));
                            return;
                        case "successful_refund_notification":
                            SuccessfulRefundNotification(new WebHookAccount(reader), new WebHookTransaction(reader));
                            return;
                        case "void_payment_notification":
                            VoidPaymentNotification(new WebHookAccount(reader), new WebHookTransaction(reader));
                            return;
                            
                        case "new_invoice_notification":
                            NewInvoiceNotification(new WebHookAccount(reader), new WebHookInvoice(reader));
                            return;
                        case "closed_invoice_notification":
                            ClosedInvoiceNotification(new WebHookAccount(reader), new WebHookInvoice(reader));
                            return;
                        case "past_due_invoice_notification":
                            PastDueInvoiceNotification(new WebHookAccount(reader), new WebHookInvoice(reader));
                            return;
                        case "fraud_info_updated_notification":
                            FraudInfoUpdatedNotification(new WebHookAccount(reader), new WebHookTransaction(reader));
                            return;
                        default:
                            Trace.TraceError("{0}: ReadXml - unknown message received {1}", DateTime.UtcNow.ToLog(), reader.Name);
                            break;
                    }
                }
            }
        }

    }
}