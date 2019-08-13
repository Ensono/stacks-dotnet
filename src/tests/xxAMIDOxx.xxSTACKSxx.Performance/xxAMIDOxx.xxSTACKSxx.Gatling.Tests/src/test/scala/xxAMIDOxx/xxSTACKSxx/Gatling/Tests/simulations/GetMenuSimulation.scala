package xxAMIDOxx.xxSTACKSxx.Gatling.Tests.simulations

import io.gatling.core.Predef.Simulation
import io.gatling.core.Predef._
import scala.concurrent.duration._
import xxAMIDOxx.xxSTACKSxx.Gatling.Tests.scenarios.GetMenuScenario
import xxAMIDOxx.xxSTACKSxx.Gatling.Tests.config.Config._

//Simulations are where you define the load that will be injected to the server
//Docs: https://gatling.io/docs/current/general/simulation_structure/
//Docs: https://gatling.io/docs/current/general/simulation_setup
class GetMenuSimulation extends Simulation {
  /*private val getMenuExec = GetMenuScenario.getMenuScenario
    .inject(atOnceUsers(users))

  setUp(getMenuExec)*/

  private val getMenuRampExec = GetMenuScenario.getMenuScenario
    .inject(rampUsers(users) during(10 seconds))

  setUp(getMenuRampExec)
}
