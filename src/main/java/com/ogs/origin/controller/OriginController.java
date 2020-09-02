package com.ogs.origin.controller;

import com.ogs.origin.exception.OriginException;
import com.ogs.origin.model.PersonalInformation;
import com.ogs.origin.service.OriginService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/risk-calculation")
public class OriginController {

    @Autowired
    private OriginService originService;

    @PostMapping
    private ResponseEntity<?> calculateRisk(@RequestBody PersonalInformation personalInformation){
        try {
            return new ResponseEntity<>(originService.calculateRisk(personalInformation), HttpStatus.CREATED);
        } catch (OriginException e) {
            e.printStackTrace();
            return new ResponseEntity<>(e.getMessage(), HttpStatus.BAD_REQUEST);
        }
    }
}
