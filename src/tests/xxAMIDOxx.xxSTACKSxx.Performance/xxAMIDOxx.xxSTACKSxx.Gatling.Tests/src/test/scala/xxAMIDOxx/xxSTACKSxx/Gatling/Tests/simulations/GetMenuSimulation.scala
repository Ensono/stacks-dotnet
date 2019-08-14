package xxAMIDOxx.xxSTACKSxx.Gatling.Tests.simulations

import io.gatling.core.Predef._
import scala.concurrent.duration._
import xxAMIDOxx.xxSTACKSxx.Gatling.Tests.scenarios.GetMenuScenario
import xxAMIDOxx.xxSTACKSxx.Gatling.Tests.config.Config._

//Simulations are where you define the load that will be injected to the server
//Docs: https://gatling.io/docs/current/general/simulation_structure/
//Docs: https://gatling.io/docs/current/general/simulation_setup
class GetMenuSimulation extends Simulation {
  private val getMenuRampExec = GetMenuScenario.getMenuScenario
    .inject(
      //rampConcurrentUsers(constantusers) to (maxusers) during (rampUp seconds),
      //constantConcurrentUsers(constantusers) during (duration seconds),
      rampUsers(maxusers) during (rampup seconds)
      //nothingFor(4 seconds),
      //atOnceUsers(10),
      //rampUsers(10) during (5 seconds),
      //constantUsersPerSec(20) during (15 seconds),
      //constantUsersPerSec(20) during (15 seconds) randomized,
      //rampUsersPerSec(10) to 20 during (10 minutes),
      //rampUsersPerSec(10) to 20 during (10 minutes) randomized,
      //heavisideUsers(1000) during (20 seconds)
    )

  setUp(getMenuRampExec)
}
