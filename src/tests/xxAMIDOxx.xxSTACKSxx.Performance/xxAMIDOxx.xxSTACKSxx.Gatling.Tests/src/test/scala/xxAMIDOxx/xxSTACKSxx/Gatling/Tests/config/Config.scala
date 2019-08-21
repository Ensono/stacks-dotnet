package xxAMIDOxx.xxSTACKSxx.Gatling.Tests.config

import com.typesafe.config._

//This is where you set configuration values for the project
object Config {
  val environment: String =  System.getProperty("env")
  private val config: Config = ConfigFactory.load(s"$environment.application.properties")
  lazy val baseUrl: String = config.getString("baseUrl")

  val users: Int = Integer.getInteger("users", 1).toInt
  val rampup: Int = Integer.getInteger("rampup", 1).toInt
}
