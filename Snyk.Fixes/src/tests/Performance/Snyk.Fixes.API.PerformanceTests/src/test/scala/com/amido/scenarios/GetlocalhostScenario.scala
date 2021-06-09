package com.amido.scenarios

import com.amido.requests.GetlocalhostRequest
import io.gatling.core.Predef.scenario
import io.gatling.core.structure.ScenarioBuilder

//Scenarios are the business scenarios that will be run in the load tests. Scenarios execute requests.
//Docs: https://gatling.io/docs/current/general/scenario
object GetlocalhostScenario {
  val getlocalhostScenario: ScenarioBuilder = scenario("Get localhost scenario")
    .exec(GetlocalhostRequest.get_localhost)
}
