package xxAMIDOxx.xxSTACKSxx.API.PerformanceTests.Gatling.requests

import java.util.UUID.randomUUID
import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.http.request.builder.HttpRequestBuilder
import xxAMIDOxx.xxSTACKSxx.API.PerformanceTests.config.Config._

object CreateMenuRequest {
  var create_menu: HttpRequestBuilder = http("Create Menu")
    .post(baseUrl + "/v1/menu")
    .body(RawFileBody("./resources/bodies/CreateMenu.json")).asJson
    .header("content-type", "application/json")
    .check(status is 201)
}