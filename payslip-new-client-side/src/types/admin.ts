export type AdminLocation = {
    location: string,
    coordinates: AdminLocationCordinate;
    facilities_summary: AdminLocationFacility;
    search_radius_meters: number;
}

export type AdminLocationFacility = {
    healthcare: number;
    education: number;
    transport: number;
    shopping: number;
    restaurants: number;
}

export type AdminLocationCordinate = {
    latitude: number;
    longitude: number;
}

export type AdminLocationEntry = {
    location: string
}