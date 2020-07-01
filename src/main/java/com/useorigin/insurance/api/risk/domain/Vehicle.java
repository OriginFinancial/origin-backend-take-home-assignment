package com.useorigin.insurance.api.risk.domain;

import java.time.Year;

public class Vehicle {

    private final Year year;

    public Vehicle(Year year) {
        this.year = year;
    }

    public Year getYear() {
        return year;
    }
}
