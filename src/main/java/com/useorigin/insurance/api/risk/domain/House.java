package com.useorigin.insurance.api.risk.domain;

import com.fasterxml.jackson.databind.PropertyNamingStrategy;
import com.fasterxml.jackson.databind.annotation.JsonNaming;

@JsonNaming(PropertyNamingStrategy.SnakeCaseStrategy.class)
public class House {

    private String ownershipStatus;

    public House() {
    }

    public House(String ownershipStatus) {
        this.ownershipStatus = ownershipStatus;
    }

    public String getOwnershipStatus() {
        return ownershipStatus;
    }

    @Override
    public String toString() {
        return "House{" +
                "ownershipStatus=" + ownershipStatus +
                '}';
    }
}
