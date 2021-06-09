package com.amido.scenarios

import com.amido.requests.CreatelocalhostRequest
import io.gatling.core.Predef.scenario
import io.gatling.core.structure.ScenarioBuilder

object CreatelocalhostScenario {
  val createlocalhostScenario: ScenarioBuilder = scenario("Get localhost scenario")
    .exec(CreatelocalhostRequest.create_localhost)
}
