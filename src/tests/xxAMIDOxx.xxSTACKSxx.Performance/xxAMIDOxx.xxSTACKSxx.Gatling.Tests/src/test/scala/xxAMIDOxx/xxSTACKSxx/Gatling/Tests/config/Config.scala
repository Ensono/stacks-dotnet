package xxAMIDOxx.xxSTACKSxx.Gatling.Tests.config

//This is where you set configuration values for the project
object Config {
  val app_url = "http://devx.azure.amidostacks.com/api/menu"

  val users: Int = Integer.getInteger("users", 10).toInt
  val rampUp: Int = Integer.getInteger("rampup", 1).toInt
  val throughput: Int = Integer.getInteger("throughput", 100).toInt
}
