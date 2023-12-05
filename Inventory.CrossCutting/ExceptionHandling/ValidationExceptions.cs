using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.CrossCutting.ExceptionHandling
{

    public abstract class InventoryExceptionBase : Exception
    {
        public InventoryExceptionBase() : base() { }
        public InventoryExceptionBase(string message) : base(message)
        {
            
        }
    }

    public class NotSavedException : InventoryExceptionBase
    {
        public NotSavedException() : base() { }
        public NotSavedException(string message) : base(message) { }
    }

    public class InternalServerError : InventoryExceptionBase
    {
        public InternalServerError() : base() { }
        public InternalServerError(string message) : base(message) { }
    }
    public class InvalidDeactivationException : InventoryExceptionBase
    {
        public InvalidDeactivationException() : base() { }
        public InvalidDeactivationException(string message) : base(message) { }
    }
    public class InvalidModelStateException : InventoryExceptionBase
    {
        public InvalidModelStateException() : base() { }
        public InvalidModelStateException(string message) : base(message) { }
    }
    public class InCompeleteException : InventoryExceptionBase
    {
        public InCompeleteException() : base() { }
        public InCompeleteException(string message) : base(message) { }
    }
    public class InvalidException : InventoryExceptionBase
    {
        public InvalidException() : base() { }
        public InvalidException(string message) : base(message) { }
    }

    public class MoreThanTwoException : InventoryExceptionBase
    {
        public MoreThanTwoException() : base() { }
        public MoreThanTwoException(string message) : base(message) { }
    }
    public class MoreeThanOneException : InventoryExceptionBase
    {
        public MoreeThanOneException() : base() { }
        public MoreeThanOneException(string message) : base(message) { }
    }
    public class WrongValueException : InventoryExceptionBase
    {
        public WrongValueException() : base() { }
        public WrongValueException(string message) : base(message) { }
    }
    public class NullInputsException : InventoryExceptionBase
    {
        public NullInputsException() : base() { }
        public NullInputsException(string message) : base(message) { }
    }
    public class NullSearchValueException : InventoryExceptionBase
    {
        public NullSearchValueException() : base() { }
        public NullSearchValueException(string message) : base(message) { }
    }
    public class UnAuthorizedException : InventoryExceptionBase
    {
        public UnAuthorizedException() : base() { }
        public UnAuthorizedException(string message) : base(message) { }
    }
    public class InvalidUserNameOrPasswordException : InventoryExceptionBase
    {
        public InvalidUserNameOrPasswordException() : base() { }
        public InvalidUserNameOrPasswordException(string message) : base(message) { }
    }

    public class InvalidBookException : InventoryExceptionBase
    {
        public InvalidBookException() : base() { }
        public InvalidBookException(string message) : base(message) { }
    }
    public class InvalidRemainException : InventoryExceptionBase
    {
        public InvalidRemainException() : base() { }
        public InvalidRemainException(string message) : base(message) { }
    }
    public class InvalidStoreItemStatusException : InventoryExceptionBase
    {
        public InvalidStoreItemStatusException() : base() { }
        public InvalidStoreItemStatusException(string message) : base(message) { }
    }
    public class InvalidInvoiceException : InventoryExceptionBase
    {
        public InvalidInvoiceException() : base() { }
        public InvalidInvoiceException(string message) : base(message) { }
    }

    public class NoDataException : InventoryExceptionBase
    {
        public NoDataException() : base() { }
        public NoDataException(string message) : base(message) { }
    }
    public class UserHasNoTenantException : InventoryExceptionBase
    {
        public UserHasNoTenantException() : base() { }
        public UserHasNoTenantException(string message) : base(message) { }
    }
    
    public class InvalidEditExaminationBudget : InventoryExceptionBase
    {
        public InvalidEditExaminationBudget() : base() { }
        public InvalidEditExaminationBudget(string message) : base(message) { }
    }
    public class InvalidCanceledExchangeOrder : InventoryExceptionBase
    {
        public InvalidCanceledExchangeOrder() : base() { }
        public InvalidCanceledExchangeOrder(string message) : base(message) { }
    }
    public class NoChangesInEdit : InventoryExceptionBase
    {
        public NoChangesInEdit() : base() { }
        public NoChangesInEdit(string message) : base(message) { }
    }
    public class NotValidTenant : InventoryExceptionBase
    {
        public NotValidTenant() : base() { }
        public NotValidTenant(string message) : base(message) { }
    }

    public class ReviewException : InventoryExceptionBase
    {
        public ReviewException() : base() { }
        public ReviewException(string message) : base(message) { }
    }

    public class InvalidCanceledTransformation : InventoryExceptionBase
    {
        public InvalidCanceledTransformation() : base() { }
        public InvalidCanceledTransformation(string message) : base(message) { }
    }


    public class InvalidCanceledRobbingOrder : InventoryExceptionBase
    {
        public InvalidCanceledRobbingOrder() : base() { }
        public InvalidCanceledRobbingOrder(string message) : base(message) { }
    }


    public class InvalidCanceledExecutionOrder : InventoryExceptionBase
    {
        public InvalidCanceledExecutionOrder() : base() { }
        public InvalidCanceledExecutionOrder(string message) : base(message) { }
    }


    

}
