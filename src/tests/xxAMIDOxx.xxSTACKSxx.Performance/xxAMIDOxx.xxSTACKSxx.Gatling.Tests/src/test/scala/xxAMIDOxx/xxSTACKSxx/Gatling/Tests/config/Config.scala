package xxAMIDOxx.xxSTACKSxx.Gatling.Tests.config

//This is where you set configuration values for the project
object Config {
  val app_url = "http://devx.azure.amidostacks.com/api/menu"

  val users: Int = Integer.getInteger("users", 1).toInt
  val rampup: Int = Integer.getInteger("rampup", 1).toInt
}
