using AutoFixture.Xunit2;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests;

public class CustomInlineAutoDataAttribute(params object[] values)
    : InlineAutoDataAttribute(new CustomAutoDataAttribute(), values);
