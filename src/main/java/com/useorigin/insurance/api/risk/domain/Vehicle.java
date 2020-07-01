package com.useorigin.insurance.api.risk.domain;

import com.fasterxml.jackson.annotation.JsonFormat;

import java.time.Year;

public class Vehicle {

    @JsonFormat(shape = JsonFormat.Shape.NUMBER_INT, pattern = "yyyy")
    private Year year;

    public Vehicle() {
    }

    public Vehicle(Year year) {
        this.year = year;
    }

    public Year getYear() {
        return year;
    }
}
