package xxAMIDOxx.xxSTACKSxx.API.PerformanceTests.Gatling.scenarios

import io.gatling.core.Predef.scenario
import io.gatling.core.structure.ScenarioBuilder
import xxAMIDOxx.xxSTACKSxx.API.PerformanceTests.Gatling.requests.SearchMenuRequest

//Scenarios are the business scenarios that will be run in the load tests. Scenarios execute requests.
//Docs: https://gatling.io/docs/current/general/scenario
object GetMenuScenario {
  val getMenuScenario: ScenarioBuilder = scenario("Get menu scenario")
    .exec(SearchMenuRequest.get_menu)
}
