package com.useorigin.insurance.api.risk.domain;

import java.time.Year;

public class Vehicle {

    private Integer year;

    public Vehicle(Integer year) {
        this.year = year;
    }

    public Year getYear() {
        return Year.of(year);
    }
}
