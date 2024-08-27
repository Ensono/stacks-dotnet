namespace xxENSONOxx.xxSTACKSxx.Shared.API.Tests.IntegrationTests.Middleware;

public class CorrelationIdMiddlewareTestStories
{
    private const string DEFAULT_HEADER_NAME = "x-correlation-id";
    private const string CUSTOM_HEADER_NAME = "x-custom-correlation-id";
    private const string PRESET_HEADER_VALUE = "already-set";
    private const string CORRELATION_ID_LOG_CONTEXT_PROPERTY = "CorrelationId";


    [Theory, AutoData]
    public void MiddlewareAddsRequestHeaderWhenOneHasNotYetBeenSet(CorrelationIdMiddlewareTestSteps steps)
    {
        this.Given(_   => steps.GivenTheConfigurationFileDoesNotHaveTheHeaderNameSet())
                .And(_ => steps.GivenTheConfigurationFileHasIncludeInResponseSetTo(false))
                .And(_ => steps.GivenTheRequestHeaderHasNotAlreadyBeenSet())
            .When(_    => steps.WhenRequestIsSentToTheServer())
            .Then(_    => steps.ThenRequestHeaderIsAddedWithTheName(DEFAULT_HEADER_NAME))
                .And(_ => steps.ThenResponseHeaderIsNotAddedWithTheName(DEFAULT_HEADER_NAME))
                .And(_ => steps.ThenLogsIncludePropertyWithTheName(DEFAULT_HEADER_NAME))
            .BDDfy();
    }


    [Theory, AutoData]
    public void MiddlewareAddsRequestHeaderWhenOneHasNotYetBeenSetUsingConfigurationName(CorrelationIdMiddlewareTestSteps steps)
    {
        this.Given(_   => steps.GivenTheConfigurationFileHasTheHeaderNameSetTo(CUSTOM_HEADER_NAME))
                .And(_ => steps.GivenTheConfigurationFileHasIncludeInResponseSetTo(false))
                .And(_ => steps.GivenTheRequestHeaderHasNotAlreadyBeenSet())
            .When(_    => steps.WhenRequestIsSentToTheServer())
            .Then(_    => steps.ThenRequestHeaderIsAddedWithTheName(CUSTOM_HEADER_NAME))
                .And(_ => steps.ThenRequestHeaderIsNotAddedWithTheName(DEFAULT_HEADER_NAME))
                .And(_ => steps.ThenResponseHeaderIsNotAddedWithTheName(CUSTOM_HEADER_NAME))
                .And(_ => steps.ThenLogsIncludePropertyWithTheName(CUSTOM_HEADER_NAME))
            .BDDfy();
    }


    [Theory, AutoData]
    public void MiddlewareAddsResponseHeaderWhenConfiguredToInclude(CorrelationIdMiddlewareTestSteps steps)
    {
        this.Given(_   => steps.GivenTheConfigurationFileDoesNotHaveTheHeaderNameSet())
                .And(_ => steps.GivenTheConfigurationFileHasIncludeInResponseSetTo(true))
                .And(_ => steps.GivenTheRequestHeaderHasNotAlreadyBeenSet())
            .When(_    => steps.WhenRequestIsSentToTheServer())
            .Then(_    => steps.ThenRequestHeaderIsAddedWithTheName(DEFAULT_HEADER_NAME))
                .And(_ => steps.ThenResponseHeaderIsAddedWithTheName(DEFAULT_HEADER_NAME))
                .And(_ => steps.ThenLogsIncludePropertyWithTheName(DEFAULT_HEADER_NAME))
            .BDDfy();
    }


    [Theory, AutoData]
    public void MiddlewareAddsResponseHeaderWhenConfiguredToIncludeUsingConfigurationName(CorrelationIdMiddlewareTestSteps steps)
    {
        this.Given(_   => steps.GivenTheConfigurationFileHasTheHeaderNameSetTo(CUSTOM_HEADER_NAME))
                .And(_ => steps.GivenTheConfigurationFileHasIncludeInResponseSetTo(true))
                .And(_ => steps.GivenTheRequestHeaderHasNotAlreadyBeenSet())
            .When(_    => steps.WhenRequestIsSentToTheServer())
            .Then(_    => steps.ThenRequestHeaderIsAddedWithTheName(CUSTOM_HEADER_NAME))
                .And(_ => steps.ThenRequestHeaderIsNotAddedWithTheName(DEFAULT_HEADER_NAME))
                .And(_ => steps.ThenResponseHeaderIsAddedWithTheName(CUSTOM_HEADER_NAME))
                .And(_ => steps.ThenResponseHeaderIsNotAddedWithTheName(DEFAULT_HEADER_NAME))
                .And(_ => steps.ThenLogsIncludePropertyWithTheName(CUSTOM_HEADER_NAME))
            .BDDfy();
    }


    [Theory, AutoData]
    public void MiddlewareDoesNotAdjustRequestHeaderWhenOneHasNotAlreadyBeenSet(CorrelationIdMiddlewareTestSteps steps)
    {
        this.Given(_   => steps.GivenTheConfigurationFileDoesNotHaveTheHeaderNameSet())
                .And(_ => steps.GivenTheConfigurationFileHasIncludeInResponseSetTo(true))
                .And(_ => steps.GivenTheRequestHeaderHasAlreadyBeenSet(DEFAULT_HEADER_NAME, PRESET_HEADER_VALUE))
            .When(_    => steps.WhenRequestIsSentToTheServer())
            .Then(_    => steps.ThenRequestHeaderIsAddedWithTheNameAndValue(DEFAULT_HEADER_NAME, PRESET_HEADER_VALUE))
                .And(_ => steps.ThenResponseHeaderIsAddedWithTheNameAndValue(DEFAULT_HEADER_NAME, PRESET_HEADER_VALUE))
                .And(_ => steps.ThenLogsIncludePropertyWithTheNameAndValue(DEFAULT_HEADER_NAME, PRESET_HEADER_VALUE))
            .BDDfy();
    }


    [Theory, AutoData]
    public void MiddlewareDoesNotAdjustRequestHeaderWithConfiguredNameWhenOneHasNotAlreadyBeenSet(CorrelationIdMiddlewareTestSteps steps)
    {
        this.Given(_   => steps.GivenTheConfigurationFileHasTheHeaderNameSetTo(CUSTOM_HEADER_NAME))
            .And(_ => steps.GivenTheConfigurationFileHasIncludeInResponseSetTo(true))
                .And(_ => steps.GivenTheRequestHeaderHasAlreadyBeenSet(CUSTOM_HEADER_NAME, PRESET_HEADER_VALUE))
            .When(_    => steps.WhenRequestIsSentToTheServer())
            .Then(_    => steps.ThenRequestHeaderIsAddedWithTheNameAndValue(CUSTOM_HEADER_NAME, PRESET_HEADER_VALUE))
                .And(_ => steps.ThenRequestHeaderIsNotAddedWithTheName(DEFAULT_HEADER_NAME))
                .And(_ => steps.ThenResponseHeaderIsAddedWithTheNameAndValue(CUSTOM_HEADER_NAME, PRESET_HEADER_VALUE))
                .And(_ => steps.ThenResponseHeaderIsNotAddedWithTheName(DEFAULT_HEADER_NAME))
                .And(_ => steps.ThenLogsIncludePropertyWithTheNameAndValue(CUSTOM_HEADER_NAME, PRESET_HEADER_VALUE))
            .BDDfy();
    }


    [Theory, AutoData]
    public void CorrelationIdLogContextPropertyIsNotNullWhenAddedOnlyOnce(CorrelationIdMiddlewareTestSteps steps)
    {
        //  A previous issue in .Net Core 2.1 replaced the Correlation ID with a NULL value.
        //  The work around fr this issue has been removed as that issue cannot be reproduced.
        //  This test is in place to ensure it doesn't re-occur.
        this.Given(_   => steps.GivenTheConfigurationFileDoesNotHaveTheHeaderNameSet())
                .And(_ => steps.GivenSerilogIsConfiguredToEnrichLogsWithCorrelationId())
            .When(_    => steps.WhenRequestIsSentToTheServer())
            .Then(_    => steps.ThenRequestHeaderIsAddedWithTheName(DEFAULT_HEADER_NAME))
                .And(_ => steps.ThenLogsIncludePropertyWithTheName(DEFAULT_HEADER_NAME))
                .And(_ => steps.ThenLogsIncludePropertyWithTheName(CORRELATION_ID_LOG_CONTEXT_PROPERTY))
            .BDDfy();
    }
}
