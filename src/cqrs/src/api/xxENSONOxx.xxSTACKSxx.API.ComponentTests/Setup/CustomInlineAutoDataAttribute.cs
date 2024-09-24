using AutoFixture.Xunit2;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Setup;

public class CustomInlineAutoDataAttribute(params object[] values)
    : InlineAutoDataAttribute(new CustomAutoDataAttribute(), values);
