/**********************************************************************
* Copyright (c) 1999-2014 Infratel, Inc.
* All rights reserved.
* http://www.infratel.com
*
***********************************************************************/

using System;
using System.Xml;
using System.Collections.Generic;
using Recurly;

namespace Infratel.RecurlyLibrary
{
    public class WebHookTransaction : RecurlyWebHookEntity
    {
        public string Id { get; private set; }
        public string InvoiceId { get; private set; }
        public int InvoiceNumber { get; private set; }
        public string SubscriptionId { get; private set; }
        public Recurly.Transaction.TransactionType Action { get; set; }
        public DateTime? Date { get; private set; }
        public int AmountInCents { get; set; }
        public Recurly.Transaction.TransactionState Status { get; private set; }
        public string Message { get; private set; }
        public string Reference { get; private set; }
        public string Source { get; private set; }
        public string CVVResult { get; private set; }
        public string AvsResult { get; private set; }
        public string AvsResultStreet { get; private set; }
        public string AvsResultPostal { get; private set; }
        public bool Test { get; private set; }
        public bool Voidable { get; private set; }
        public bool Refundable { get; private set; }
        public bool ManuallyEntered { get; private set; }
        public string PaymentMethod { get; private set; }

        public int GatewayErrorCode { get; private set; }
        public FailureType FailureType { get; private set; }

        internal WebHookTransaction(XmlTextReader xmlReader)
        {
            ReadXml(xmlReader);
        }

        internal override void ReadXml(XmlTextReader reader)
        {
            while (reader.Read())
            {
                // End of account element, get out of here
                if ((reader.Name == "transaction") &&
                    reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType != XmlNodeType.Element) continue;
                
                int amount;
                switch (reader.Name)
                {
                    case "id":
                        Id = reader.ReadElementContentAsString();
                        break;

                    case "invoice_id":
                        InvoiceId = reader.ReadElementContentAsString();
                        break;

                    case "invoice_number":
                        int invNumber;
                        if (Int32.TryParse(reader.ReadElementContentAsString(), out invNumber))
                            InvoiceNumber = invNumber;
                        break;

                    case "subscription_id":
                        SubscriptionId = reader.ReadElementContentAsString();
                        break;

                    case "action":
                        Action = reader.ReadElementContentAsString().ParseAsEnum<Transaction.TransactionType>();
                        break;

                    case "date":
                        DateTime createdAt;
                        if (DateTime.TryParse(reader.ReadElementContentAsString(), out createdAt))
                            Date = createdAt;
                        break;

                    case "amount_in_cents":
                        if (Int32.TryParse(reader.ReadElementContentAsString(), out amount))
                            AmountInCents = amount;
                        break;

                    case "status":
                        var state = reader.ReadElementContentAsString();
                        Status = "void" == state
                                     ? Transaction.TransactionState.Voided
                                     : state.ParseAsEnum<Transaction.TransactionState>();
                        break;

                    case "message":
                        Message = reader.ReadElementContentAsString();
                        break;

                    case "reference":
                        Reference = reader.ReadElementContentAsString();
                        break;

                    case "source":
                        Source = reader.ReadElementContentAsString();
                        break;

                    case "cvv_result":
                        CVVResult = reader.ReadElementContentAsString();
                        break;

                    case "avs_result":
                        AvsResult = reader.ReadElementContentAsString();
                        break;

                    case "avs_result_street":
                        AvsResultStreet = reader.ReadElementContentAsString();
                        break;

                    case "avs_result_postal":
                        AvsResultPostal = reader.ReadElementContentAsString();
                        break;

                    case "test":
                        Test = reader.ReadElementContentAsBoolean();
                        break;

                    case "voidable":
                        Voidable = reader.ReadElementContentAsBoolean();
                        break;

                    case "refundable":
                        Refundable = reader.ReadElementContentAsBoolean();
                        break;

                    case "manually_entered":
                        ManuallyEntered = reader.ReadElementContentAsBoolean();
                        break;

                    case "payment_method":
                        PaymentMethod = reader.ReadElementContentAsString();
                        break;

                    case "gateway_error_codes":
                        {
                            int code;
                            var value = reader.ReadElementContentAsString();
                            if(int.TryParse(value, out code))
                            {
                                GatewayErrorCode = code;

                            }


                        }
                        break;
                    case "failure_type":
                        {
                            string value = reader.ReadElementContentAsString();
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                FailureType = new FailureType(value);
                            }
                        }
                        break;
                }
            }
        }

        public override string ToString()
        {
            return "Recurly WebHookTransaction: " + Id;
        }
    }

    public class FailureType {

        private string _customCustomerMessage;
        private string _customMerchantMessage;

        public string CustomerMessage {

            get
            {
                if (!string.IsNullOrEmpty(_customCustomerMessage))
                    return _customCustomerMessage;

                if (Messages.ContainsKey(Failure))
                    return Messages[Failure][0];

                return null;
            }
            set
            {
                _customCustomerMessage = value;
            }
        }
        public string MerchantMessage
        {
            get
            {
                if (!string.IsNullOrEmpty(_customMerchantMessage))
                    return _customMerchantMessage;

                if (Messages.ContainsKey(Failure))
                    return Messages[Failure][0];

                return null;
            }

            set
            {
                _customMerchantMessage = value;
            }

        }

        public string Failure { get; set; }
        private static Dictionary<string, string[]> Messages = new Dictionary<string, string[]>
        {
            { "approved", new string[] {"Approved" , "Approved" } },
            { "approved_fraud_review", new string[] { "Approved", "Approved, flagged for fraud review in your payment gateway." } },
            {"declined", new string[] {"The transaction was declined. Please use a different card or contact your bank.","The transaction was declined without specific information. Please contact your payment gateway for more details or ask the customer to contact their bank."}},
            {"declined_saveable", new string[] {"The transaction was declined. Please use a different card or contact your bank.","The transaction was declined without specific information. Please contact your payment gateway for more details or ask the customer to contact their bank."}},
            {"insufficient_funds", new string[] {"The transaction was declined due to insufficient funds in your account. Please use a different card or contact your bank.","The card has insufficient funds to cover the cost of the transaction."}},
            {"temporary_hold", new string[] {"Your card has a temporary hold. Please use a different card or contact your bank.","The issuing bank has a temporary hold on the card. This is known as a 'Do Not Honor' response."}},
            {"too_many_attempts", new string[] {"The transaction was declined. You have exceeded a reasonable number of attempts. Please wait a while before retrying your card, or try a different card.","The transaction was declined because of too many authorization attempts."}},
            {"call_issuer", new string[] {"Your transaction was declined. Please contact your bank for further details or try another card.","The transaction was declined by the payment gateway. Contact the card issuer for more details."}},
            {"call_issuer_update_cardholder_data", new string[] {"Please contact support: the payment system experienced an unspecified error with your card issuer.","The transaction was declined by the payment gateway. Contact the card issuer for more details."}},
            {"paypal_primary_declined", new string[] {"Your primary funding source was declined. Please try again or update your billing information with PayPal.","The primary funding source failed but the transaction can be attempted again. The next attempt will use the alternate funding source."}},
            {"paypal_declined_use_alternate", new string[] {"Your primary funding source was declined. Please try again to use your secondary funding source.","The primary funding source failed but the transaction can be attempted again. The next attempt will use the alternate funding source."}},
            {"declined_security_code", new string[] {"The security code you entered does not match. Please update the CVV and try again.","The payment gateway declined the transaction because the security code (CVV) did not match."}},
            {"declined_exception", new string[] {"The transaction was declined. Please try again or try another card.","An exception occurred with your payment gateway while processing this transaction. The transaction may be missing required information or the information is not properly formatted. Please contact your gateway for details."}},
            {"declined_missing_data", new string[] {"Your billing information is missing some required information.","The payment gateway declined the transaction for missing a required field. Please verify your configuration with Recurly and your gateway is correct. You may need to require more address information."}},
            {"invalid_data", new string[] {"The transaction was declined due to invalid data.","The payment gateway declined the transaction due invalid data. Please check the response details for more information."}},
            {"invalid_email", new string[] {"Your email address is not valid.","The payment gateway requires a valid email address for this transaction."}},
            {"declined_card_number", new string[] {"Your card number is not valid. Please update your card number.","The credit card number is not valid. The customer needs to try a different number."}},
            {"invalid_card_number", new string[] {"Your card number is not valid. Please update your card number.","The credit card number is not valid. The customer needs to try a different number."}},
            {"invalid_account_number", new string[] {"Your account number is not valid. Please update your account number.","The account number is not valid. The customer needs to try a different number."}},
            {"gateway_token_not_found", new string[] {"Your payment details were not found. Please update your billing information.","The payment details were not found in your payment gateway. The customer needs to update their billing information."}},
            {"expired_card", new string[] {"Your credit card is expired, please update your card.","The payment gateway declined the transaction because the expiration date is expired or does not match."}},
            {"declined_expiration_date", new string[] {"Your expiration date is invalid or does not match.","The payment gateway declined the transaction because the expiration date is expired or does not match."}},
            {"exceeds_daily_limit", new string[] {"The transaction exceeds your daily approval limit. Please contact your bank or try another card.","The transaction exceeds the cardholder's daily approval limit."}},
            {"invalid_merchant_type", new string[] {"Your card is not allowed to complete this transaction. Please try another card.","The card is not allowed to make purchases from you (e.g. a Travel only card trying to purchase electronics)."}},
            {"invalid_transaction", new string[] {"Your card is not allowed to complete this transaction. Please contact your bank or try another card.","The card type cannot perform the transaction type. The card is likely restricted. The customer needs to contact their bank for details."}},
            {"invalid_issuer", new string[] {"Your card number is not valid. Please try another card or contact your bank.","The card number references an issuer (bank) that does not exist. It is not a valid card number."}},
            {"card_type_not_accepted", new string[] {"Your card type is not accepted. Please try another card.","Your merchant account does not accept this card type or specific transaction."}},
            {"payment_not_accepted", new string[] {"Your payment type is not accepted. Please try another card.","Your merchant account does not accept this payment type or specific transaction."}},
            {"restricted_card", new string[] {"Your card cannot be accepted. Please contact your issuing bank for details or try another card.","The card number has restrictions that prevent it from being used with your merchant account. It is likely a corporate card. The customer needs to use a different card."}},
            {"restricted_card_chargeback", new string[] {"Your card cannot be accepted. Please contact your issuing bank for details or try another card.","The card has a restriction preventing approval if there are any chargebacks against it."}},
            {"card_not_activated", new string[] {"Your card has not been activated. Please call your bank to activate your card and try again.","The card is brand new and has not been activated yet."}},
            {"deposit_referenced_chargeback", new string[] {"The refund cannot be processed because of a chargeback.","The deposit is already referenced by a chargeback; therefore, a refund cannot be processed against this transaction."}},
            {"customer_canceled_transaction", new string[] {"You canceled the transaction after it was approved.Please update your billing information to authorize a new transaction.","The cardholder requested this particular payment be stopped before it settled."}},
            {"cardholder_requested_stop", new string[] {"You requested recurring payments no longer be accepted on this card.Please update your billing information.","The cardholder requested recurring payments be stopped.This card will no longer work with your merchant account."}},
            {"no_billing_information", new string[] {"Your billing information is not on file.Please add your billing information.","This transaction cannot be processed because Recurly has no billing information for this account."}},
            {"paypal_invalid_billing_agreement", new string[] {"Your PayPal billing agreement is no longer valid.Please update your billing information.","The billing agreement is no longer valid.The customer may have canceled the agreement."}},
            {"paypal_hard_decline", new string[] {"Your primary funding source was declined.Please update your billing information with PayPal or try again.","PayPal failed to run the transaction with the primary funding source."}},
            {"paypal_account_issue", new string[] {"Your primary funding source was declined.Please update your billing information with PayPal or try again.","PayPal indicated the transaction failed due to an issue with the buyer account."}},
            {"fraud_address", new string[] {"Your billing address does not match the address on your account. Please fix your address or contact your bank.","The payment gateway declined the transaction because the billing address did not match."}},
            {"fraud_security_code", new string[] {"The security code you entered does not match.Please update the CVV and try again.","The payment gateway declined the transaction because the security code (CVV) did not match."}},
            {"fraud_stolen_card", new string[] {"The transaction was declined. Please use a different card or contact your bank.","The card has been designated as lost or stolen; contact the issuing bank."}},
            {"fraud_ip_address", new string[] {"The transaction was declined. Please contact support.","The payment gateway declined the transaction because it originated from an IP address known for fraudulent transactions."}},
            {"fraud_gateway", new string[] {"The transaction was declined. Please use a different card or contact your bank.","The payment gateway declined the transaction because it originated from an IP address known for fraudulent transactions."}},
            {"fraud_too_many_attempts", new string[] {"You attempted to use this card too many times. Please wait 15 minutes before trying again, or use a different card.","The card number was used unsuccessfully too many times consecutively. The cardholder must wait before the card number will work again."}},
            {"fraud_advanced_verification", new string[] {"The transaction was declined. Please use a different card or contact your bank.","The payment gateway declined the transaction because it failed the advanced verification."}},
            {"fraud_velocity", new string[] {"The transaction was declined. Please contact support.","The transaction was part of a high-volume from a single source, indicating possible fraudulent activity."}},
            {"fraud_generic", new string[] {"Please validate information and try again. If the problem persists, please contact your bank.","The payment gateway declined the transaction because it was flagged for potential fraud. The customer needs to contact their bank or you may need to contact your gateway to learn more."}},
            {"fraud_address_recurly", new string[] {"Your billing address does not match the address on your account. Please fix your address or contact your bank.","Recurly declined the transaction because the billing address did not match the address on the bank records."}},
            {"fraud_risk_check", new string[] {"This transaction was declined because it appears to be a fraudulent attempt. Please try a different card.","Fraudulent Transaction: Recurly declined this transaction based on your fraud management settings. Please see the Fraud Details section on the transaction details page for additional information."}},
            {"fraud_manual_decision", new string[] {"This transaction was declined because it appears to be a fraudulent attempt. Please try a different card.","Fraudulent: This transaction was marked as fraudulent in Kount. Please see the Fraud Details section on the transaction for more details."}},
            {"invalid_gateway_configuration", new string[] {"Please contact support: the payment system is configured incorrectly. Your card was not charged.","Your payment gateway is not configured correctly. Please contact your payment gateway for more information."}},
            {"invalid_login", new string[] {"Please contact support: the payment system is configured incorrectly. Your card was not charged.","Your payment gateway login is incorrect. Please check your login credentials."}},
            {"gateway_unavailable", new string[] {"Please contact support: the payment system experienced an unspecified error with your card issuer.","The payment gateway was unavailable for transaction. Contact your payment gateway for more details."}},
            {"processor_unavailable", new string[] {"Please contact support: the payment system experienced an unspecified error with your card issuer.","The payment processor was unavailable for the transaction. Contact your payment gateway for more details."}},
            {"issuer_unavailable", new string[] {"Please contact support: the payment system experienced an unspecified error with your card issuer.","The issuer was unavailable to authorize the transaction. Contact the card issuer or your payment gateway for more details."}},
            {"gateway_timeout", new string[] {"Please contact support: the payment system did not respond in time to process your transaction.","The payment gateway timed out while processing this transaction. Please verify that the transaction did not process. Contact your payment gateway for more details."}},
            {"gateway_error", new string[] {"An error occurred while processing your transaction. Please contact support.","The payment gateway encountered an unknown error. Please contact your payment gateway for details."}},
            {"contact_gateway", new string[] {"Please contact support: the payment system experienced an unspecified error with your card issuer.","This error requires you to contact your payment gateway for a resolution."}},
            {"try_again", new string[] {"The payment system would like you to please try again.","The payment gateway is too busy or experienced another temporary problem, and cannot process the transaction at the moment. Please try again."}},
            {"cvv_required", new string[] {"Please contact support: the payment system experienced an error. Your card was not charged.","Your payment gateway account is requiring the security code (CVV) for all transactions. Recurly cannot process recurring/subscription transactions. Please contact your payment gateway to disable the CVV requirement."}},
            {"currency_not_supported", new string[] {"Please contact support: the requested currency is not supported for this merchant. Your card was not charged.","The currency configured on your Recurly account is not accepted by your gateway. Please use a supported currency or contact your payment gateway for more details."}},
            {"ssl_error", new string[] {"Please contact support: the payment system experienced an error. Your card was not charged.","Your PEM certificate is configured incorrectly. Recurly cannot communicate with your payment gateway."}},
            {"zero_dollar_auth_not_supported", new string[] {"Please contact support: the payment system experienced an error while authorizing your card. Your card was not charged.","Zero dollar authorizations are not supported for this card type or by your payment processor."}},
            {"no_gateway", new string[] {"Please contact support: the payment system experienced an unspecified error with your card issuer.","There is no available payment gateway on your account capable of processing this transaction."}},
            {"ach_transactions_not_supported", new string[] {"Please contact support: ACH/EFT transfers are not supported.","ACH/EFT transfers are not enabled on your account."}},
            {"three_d_secure_not_supported", new string[] {"Please contact support: the payment system experienced an error. Your card was not charged.","3D Secure was attempted but is not enabled on your account."}},
            {"transaction_not_found", new string[] {"The original transaction was not found.","The original transaction was not found."}},
            {"transaction_settled", new string[] {"The transaction has already been settled, so it cannot be voided. Please try a refund.","The transaction has already been settled, so it cannot be voided. Please try a refund."}},
            {"transaction_already_voided", new string[] {"The transaction has already been voided, so it cannot be settled or refunded.","The transaction has already been voided, so it cannot be settled or refunded."}},
            {"transaction_failed_to_settled", new string[] {"The transaction did not settle successfully. Please update your billing information.","The transaction authorized successfully, but failed to settle. The settlement request may have expired or have been canceled."}},
            {"payment_cannot_void_authorization", new string[] {"An error occurred while voiding your payment authorization. Please contact support.","The credit exceeds the amount of the original transaction."}},
            {"partial_credits_not_supported", new string[] {"An error occurred while refunding your transaction. Please contact support.","Your payment gateway does not support partially refunding a transaction. Please refund the entire amount."}},
            {"cannot_refund_unsettled_transactions", new string[] {"An error occurred while refunding your transaction. Please contact support.","The transaction has not settled yet, so it cannot be refunded. Please try voiding the transaction instead."}},
            {"transaction_cannot_be_refunded", new string[] {"The transaction cannot be refunded. Please contact support.","The transaction cannot be refunded. Please contact your payment gateway for details."}},
            {"transaction_cannot_be_voided", new string[] {"The transaction cannot be voided. Please contact support.","The transaction cannot be void. It may have settled, already been voided, or otherwise be illegible for voiding.Please contact your payment gateway for details."}},
            {"total_credit_exceeds_capture", new string[] {"An error occurred while refunding your transaction.Please contact support.","The credit exceeds the amount of the original transaction."}},
            {"authorization_expired", new string[] {"An error occurred while processing your transaction.Please contact update your billing information.","The purchase authorization expired and can no longer be used to collect payment. Please attempt another transaction."}},
            {"authorization_already_captured", new string[] {"An error occurred while processing your transaction.Please contact support.","The purchase amount was already captured. This authorization cannot be used again."}},
            {"authorization_amount_depleted", new string[] {"An error occurred while processing your transaction.Please contact support.","The amount of purchases exceeds the original authorized amount.You cannot collect more money using this authorization."}},
            {"recurly_error", new string[] {"An error occurred while processing your transaction.Please contact support.","An error occurred while processing the transaction.Please contact Recurly support."}},
            {"unknown", new string[] {"The transaction was declined or failed for an unknown reason.Please try again or contact support.","The transaction was declined or failed for an unknown reason.Please try again or contact support."}},
            {"api_error", new string[] {"An error occurred while processing your transaction.Please contact support.","The payment gateway returned an error code that usually indicates its API specs have changed.Please contact Recurly support."}},
            {"duplicate_transaction", new string[] {"A similar transaction was recently submitted.Please wait a few minutes and try again.","A transaction was recently submitted with the same Invoice Number, or the same card number and amount.The payment gateway refused to process this transaction in order to prevent a duplicate transaction."}},
            {"recurly_failed_to_get_token", new string[] {"An error occurred while initializing the transaction.Please try again.","An error occurred while initializing the transaction.Please try again."}},
            {"recurly_token_not_found", new string[] {"An error occurred while processing your transaction.Please contact support.","An error occurred while processing the transaction.Please contact Recurly support."}},
        };


        public FailureType(string type)
        {
            Failure = type;
        }
    }
}
