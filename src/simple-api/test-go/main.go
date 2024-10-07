package main

import (
	"flag"
	"fmt"
	"runtime"
)

func main() {
	// serves no purpose
	flag.Parse()

	fmt.Println("hello from tester")
	fmt.Println("Arch " + runtime.GOARCH)
	fmt.Println("OS " + runtime.GOOS)
}
