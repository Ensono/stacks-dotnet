package xxAMIDOxx.xxSTACKSxx.Gatling.Tests.simulations

import io.gatling.core.Predef._
import scala.concurrent.duration._
import xxAMIDOxx.xxSTACKSxx.Gatling.Tests.config.Config._
import xxAMIDOxx.xxSTACKSxx.Gatling.Tests.scenarios.CreateMenuScenario

class CreateMenuSimulation extends Simulation {
  private val createMenuRampExec = CreateMenuScenario.createMenuScenario
    .inject(rampUsers(users) during(rampup seconds))

  setUp(createMenuRampExec)
}
