﻿using System;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Exceptions;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Validators;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Commands;
using Shouldly;
using Xunit;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.UnitTests.Validators
{
    public class DataAnnotationValidatorTests
    {
        [Fact]
        public void GivenTheMessageValidValidatorDoesntThrowException()
        {
            var toValidate = new NotifyCommandWithAnnotation(Guid.NewGuid(), "Populated");
            var validator = new DataAnnotationValidator();

            validator.Validate(toValidate);
        }

        [Fact]
        public void GivenTheTestPropertyIsMissingThrowsMessageBodyValidationException()
        {
            var toValidate = new NotifyCommandWithAnnotation(Guid.NewGuid(), null);
            var validator = new DataAnnotationValidator();

            ShouldThrowExtensions.ShouldThrow<InvalidMessageBodyException>(() => validator.Validate(toValidate));
        }
    }
}