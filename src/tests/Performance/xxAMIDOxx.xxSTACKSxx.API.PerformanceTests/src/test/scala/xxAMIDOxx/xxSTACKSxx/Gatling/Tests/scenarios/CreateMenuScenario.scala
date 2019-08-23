package xxAMIDOxx.xxSTACKSxx.Gatling.Tests.scenarios

import io.gatling.core.Predef.scenario
import io.gatling.core.structure.ScenarioBuilder
import xxAMIDOxx.xxSTACKSxx.Gatling.Tests.requests.CreateMenuRequest

object CreateMenuScenario {
  val createMenuScenario: ScenarioBuilder = scenario("Get menu scenario")
    .exec(CreateMenuRequest.create_menu)
}
