package xxAMIDOxx.xxSTACKSxx.API.PerformanceTests.Gatling.simulations

import scala.concurrent.duration._
import io.gatling.core.Predef._
import xxAMIDOxx.xxSTACKSxx.API.PerformanceTests.Gatling.config.Config._
import xxAMIDOxx.xxSTACKSxx.API.PerformanceTests.Gatling.scenarios.CreateMenuScenario

class CreateMenuSimulation extends Simulation {
  private val createMenuRampExec = CreateMenuScenario.createMenuScenario
    .inject(rampUsers(users) during(rampup seconds))

  setUp(createMenuRampExec)
}
