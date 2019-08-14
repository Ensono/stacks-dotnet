package xxAMIDOxx.xxSTACKSxx.Gatling.Tests.config

//This is where you set configuration values for the project
object Config {
  val app_url = "http://devx.azure.amidostacks.com/api/menu"

  val constantusers: Int = Integer.getInteger("constantusers", 1).toInt
  val maxusers: Int = Integer.getInteger("maxusers", 10).toInt
  val rampup: Int = Integer.getInteger("rampup", 1).toInt
  val throughput: Int = Integer.getInteger("throughput", 100).toInt
  val duration: Int = Integer.getInteger("duration", 10).toInt
}
