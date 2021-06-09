package com.amido.simulations

import com.amido.config.Configuration._
import com.amido.scenarios.CreatelocalhostScenario
import io.gatling.core.Predef._

import scala.concurrent.duration._

class CreatelocalhostSimulation extends Simulation {
  private val createlocalhostRampExec = CreatelocalhostScenario.createlocalhostScenario
    .inject(rampUsers(users) during(rampup seconds))

  setUp(createlocalhostRampExec)
    .assertions(global.responseTime.max.lt(2000))
}
