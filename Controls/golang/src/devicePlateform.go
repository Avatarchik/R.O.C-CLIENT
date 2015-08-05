package main

import (
	"fmt"
	"github.com/hybridgroup/gobot"
	"github.com/hybridgroup/gobot/platforms/joystick"
	"math"
	"net"
	"strconv"
)

type Dev interface {
	Power() int
	Start()
}

func main() {

	// conn, err := net.Dial("tcp", "localhost:8080")
	// if err != nil {
	// 	// handle error
	// 	return
	// }
	// fmt.Fprintf(conn, "GET / HTTP/1.0\r\n\r\n")
	ln, err := net.Listen("tcp", ":8081")
	if err != nil {
		// handle error
	}
	for {
		conn, err := ln.Accept()
		if err != nil {
			// handle error
		}
		go handleConnection(conn)
	}
	// for status, err := bufio.NewReader(conn).ReadString('\n'); status != "exit\n"; status, err = bufio.NewReader(conn).ReadString('\n') {
	// 	conn.Write()
	// }
}

func handleConnection(conn net.Conn) {

	d := dualShock3{}
	end := []byte("\n")
	d.p = func(data int) {
		conn.Write(strconv.AppendInt(nil, int64(data), 10))
		conn.Write(end)
	}
	d.Start()
	for i := 0; i < 34; i = i % 2 {
	}
}

type dualShock3 struct {
	p func(dat int)
}

func (d dualShock3) Power() int {
	return 20
}

func (d dualShock3) Start() {

	gbot := gobot.NewGobot()

	joystickAdaptor := joystick.NewJoystickAdaptor("ps3")
	joystick := joystick.NewJoystickDriver(joystickAdaptor,
		"ps3",
		"./dualshock3.json",
	)

	work := func() {
		gobot.On(joystick.Event("square_press"), func(data interface{}) {
			fmt.Println("square_press")
		})
		gobot.On(joystick.Event("square_release"), func(data interface{}) {
			fmt.Println("square_release")
		})
		gobot.On(joystick.Event("triangle_press"), func(data interface{}) {
			fmt.Println("triangle_press")
		})
		gobot.On(joystick.Event("triangle_release"), func(data interface{}) {
			fmt.Println("triangle_release")
		})
		gobot.On(joystick.Event("left_x"), func(data interface{}) {
			fmt.Println("left_x", data)
		})
		gobot.On(joystick.Event("left_y"), func(data interface{}) {
			fmt.Println("left_y", data)
		})
		gobot.On(joystick.Event("right_x"), func(data interface{}) {
			fmt.Println("right_x", data)
		})
		gobot.On(joystick.Event("right_y"), func(data interface{}) {
			fmt.Println("right_y", data)
			power := int(math.Floor(float64(data.(int16)) * (1 / 327.68)))
			d.p(power)
		})
	}

	robot := gobot.NewRobot("joystickBot",
		[]gobot.Connection{joystickAdaptor},
		[]gobot.Device{joystick},
		work,
	)

	gbot.AddRobot(robot)
	gbot.Start()
}
